using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using DAL.IRepository;
using Service_ERP.DTO;
using Service_ERP.IService;
using Microsoft.EntityFrameworkCore;

namespace Service_ERP.Service
{
    public class CommandeService : ICommandeService
    {
        private readonly IRepositoryAsync<Commande> _repository;
        private readonly IMapper _mapper;

        public CommandeService(IRepositoryAsync<Commande> repository, IMapper mapper)
        {
            _repository = repository;
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
            var entity = _mapper.Map<Commande>(dto);

            await _repository.Add(entity);
            await _repository.Save();

            return _mapper.Map<CommandeDto>(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task UpdateAsync(CommandeDto dto)
        {
            var entity = _mapper.Map<Commande>(dto);

            await _repository.Update(entity);
            await _repository.Save();
        }

        // =========================
        // DELETE
        // =========================
        public async Task DeleteAsync(params object[] keyValues)
        {
            var entity = await _repository.GetById(keyValues);

            if (entity != null)
            {
                await _repository.Delete(entity);
                await _repository.Save();
            }
        }
    }
}