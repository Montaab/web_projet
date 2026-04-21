using AutoMapper;
using Core.Entities;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;
using Service_ERP.DTO;
using Service_ERP.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service_ERP.Service
{
    public class ArticleService : IArticleService
    {
        private readonly IRepositoryAsync<Article> _repository;
        private readonly IMapper _mapper;

        public ArticleService(IRepositoryAsync<Article> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<IEnumerable<ArticleDto>> GetAllAsync()
        {
            var entities = await _repository.GetMuliple(
                include: q => q
                    .Include(a => a.IdScatNavigation)
                    .Include(a => a.LCommandes)
                    .Include(a => a.LFournisseurs)
                    .Include(a => a.LPaniers)
            );

            return _mapper.Map<IEnumerable<ArticleDto>>(entities);
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<ArticleDto> GetByIdAsync(params object[] keyValues)
        {
            var entity = await _repository.GetFirstOrDefault(
                predicate: a => a.IdArt == (int)keyValues[0],
                include: q => q
                    .Include(a => a.IdScatNavigation)
                    .Include(a => a.LCommandes)
                        .ThenInclude(lc => lc.IdComNavigation)
                    .Include(a => a.LFournisseurs)
                        .ThenInclude(lf => lf.IdFourNavigation)
                    .Include(a => a.LPaniers)
                        .ThenInclude(lp => lp.IdPanNavigation)
            );

            return _mapper.Map<ArticleDto>(entity);
        }

        // =========================
        // ADD
        // =========================
        public async Task<ArticleDto> AddAsync(ArticleDto dto)
        {
            var entity = _mapper.Map<Article>(dto);

            await _repository.Add(entity);
            await _repository.Save();

            return _mapper.Map<ArticleDto>(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task UpdateAsync(ArticleDto dto)
        {
            var entity = _mapper.Map<Article>(dto);

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