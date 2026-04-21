using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using DAL.IRepository;
using Service_ERP.DTO;
using Service_ERP.IService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Service_ERP.Service
{
    public class CommandeService : ICommandeService
    {
        private readonly IRepositoryAsync<Commande> _repository;
        private readonly IRepositoryAsync<Article> _articleRepository;
        private readonly IMapper _mapper;

        public CommandeService(
            IRepositoryAsync<Commande> repository,
            IRepositoryAsync<Article> articleRepository,
            IMapper mapper)
        {
            _repository = repository;
            _articleRepository = articleRepository;
            _mapper = mapper;
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<IEnumerable<CommandeDto>> GetAllAsync()
        {
            var entities = await _repository.GetMuliple(
                include: q => q
                    .Include(x => x.IdCltNavigation)
                    .Include(x => x.Factures)
                    .Include(x => x.LCommandes)
                        .ThenInclude(lc => lc.IdArtNavigation)
            );

            return _mapper.Map<IEnumerable<CommandeDto>>(entities);
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<CommandeDto> GetByIdAsync(params object[] keyValues)
        {
            var entity = await _repository.GetFirstOrDefault(
                predicate: x => x.IdCom == (int)keyValues[0],
                include: q => q
                    .Include(x => x.IdCltNavigation)
                    .Include(x => x.Factures)
                    .Include(x => x.LCommandes)
                        .ThenInclude(lc => lc.IdArtNavigation)
            );

            return _mapper.Map<CommandeDto>(entity);
        }

        // =========================
        // ADD
        // =========================
        public async Task<CommandeDto> AddAsync(CommandeDto dto)
        {
            await ApplyDatabasePricesAsync(dto);
            var entity = _mapper.Map<Commande>(dto);
            var dbContext = _repository.DbContextCMC();

            await dbContext.Commandes.AddAsync(entity);
            await dbContext.SaveChangesAsync();

            return _mapper.Map<CommandeDto>(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task UpdateAsync(CommandeDto dto)
        {
            await ApplyDatabasePricesAsync(dto);
            var dbContext = _repository.DbContextCMC();
            
            var entity = await dbContext.Commandes
                .Include(c => c.LCommandes)
                .AsTracking()
                .FirstOrDefaultAsync(c => c.IdCom == dto.IdCom);

            if (entity == null) return;

            // Mise à jour de l'en-tête
            entity.DateCom = dto.DateCom;
            entity.Statut = dto.Statut;
            entity.ModePaiement = dto.ModePaiement;
            entity.IdClt = dto.IdClt;
            entity.Total = dto.Total;

            // Synchronisation des lignes
            var dtoLines = dto.LCommandes?.ToList() ?? new List<LCommandeDto>();
            var dtoLineIds = dtoLines.Select(l => l.IdArt).ToHashSet();

            // 1. Suppression des lignes retirées
            var existingLines = entity.LCommandes.ToList();
            foreach (var exLine in existingLines)
            {
                if (!dtoLineIds.Contains(exLine.IdArt))
                {
                    dbContext.LCommandes.Remove(exLine);
                }
            }

            // 2. Mise à jour ou ajout des lignes
            foreach (var lineDto in dtoLines)
            {
                var existingLine = entity.LCommandes.FirstOrDefault(l => l.IdArt == lineDto.IdArt);

                if (existingLine != null)
                {
                    existingLine.Quantite = lineDto.Quantite;
                    existingLine.PrixAchat = lineDto.PrixAchat;
                    existingLine.Remise = lineDto.Remise;
                }
                else
                {
                    // Utiliser dbContext.LCommandes.Add pour être plus explicite
                    var newLine = new LCommande
                    {
                        IdCom = entity.IdCom,
                        IdArt = lineDto.IdArt,
                        Quantite = lineDto.Quantite,
                        PrixAchat = lineDto.PrixAchat,
                        Remise = lineDto.Remise
                    };
                    await dbContext.LCommandes.AddAsync(newLine);
                }
            }

            await dbContext.SaveChangesAsync();
        }

        // =========================
        // DELETE
        // =========================
        public async Task DeleteAsync(params object[] keyValues)
        {
            var id = (int)keyValues[0];
            var dbContext = _repository.DbContextCMC();
            var entity = await dbContext.Commandes
                .Include(c => c.Factures)
                .Include(c => c.LCommandes)
                .FirstOrDefaultAsync(c => c.IdCom == id);

            if (entity != null)
            {
                if (entity.Factures.Count > 0)
                {
                    dbContext.Factures.RemoveRange(entity.Factures);
                }

                if (entity.LCommandes.Count > 0)
                {
                    dbContext.LCommandes.RemoveRange(entity.LCommandes);
                }

                dbContext.Commandes.Remove(entity);
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task ApplyDatabasePricesAsync(CommandeDto dto)
        {
            var lines = dto.LCommandes?.ToList() ?? new List<LCommandeDto>();

            if (lines.Count == 0)
            {
                dto.Total = 0;
                dto.LCommandes = lines;
                return;
            }

            var articleIds = lines
                .Select(l => l.IdArt)
                .Distinct()
                .ToList();

            var articles = await _articleRepository.GetMuliple(
                predicate: a => articleIds.Contains(a.IdArt)
            );

            var articlePrices = articles.ToDictionary(a => a.IdArt, a => a.PrixUnitaire ?? 0m);

            decimal total = 0m;
            foreach (var line in lines)
            {
                if (!articlePrices.TryGetValue(line.IdArt, out var databasePrice))
                {
                    throw new InvalidOperationException($"Article introuvable: {line.IdArt}");
                }

                line.PrixAchat = databasePrice;
                line.Remise ??= 0m;
                line.Quantite ??= 0;
                total += (databasePrice * line.Quantite.Value) * (1 - (line.Remise.Value / 100m));
            }

            dto.Total = total;
            dto.LCommandes = lines;
        }

        private static void RecalculateTotal(CommandeDto dto)
        {
            var lines = dto.LCommandes?.ToList() ?? new List<LCommandeDto>();
            decimal total = 0m;

            foreach (var line in lines)
            {
                line.Remise ??= 0m;
                line.Quantite ??= 0;
                line.PrixAchat ??= 0m;
                total += (line.PrixAchat.Value * line.Quantite.Value) * (1 - (line.Remise.Value / 100m));
            }

            dto.Total = total;
            dto.LCommandes = lines;
        }
    }
}
