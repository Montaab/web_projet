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
    public class LPanierService : ILPanierService
    {
        private readonly IRepositoryAsync<LPanier> _repository;
        private readonly IMapper _mapper;

        public LPanierService(IRepositoryAsync<LPanier> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LPanierDto>> GetAllAsync()
        {
            var entities = await _repository.GetMuliple(
                include: q => q
                    .Include(x => x.IdArtNavigation)
                    .Include(x => x.IdPanNavigation)
            );

            return _mapper.Map<IEnumerable<LPanierDto>>(entities);
        }

        public async Task<LPanierDto> GetByIdAsync(params object[] keyValues)
        {
            var entity = await _repository.GetFirstOrDefault(
                predicate: x => x.IdPan == (int)keyValues[0] 
                             && x.IdArt == (int)keyValues[1],
                include: q => q
                    .Include(x => x.IdArtNavigation)
                    .Include(x => x.IdPanNavigation)
            );

            return _mapper.Map<LPanierDto>(entity);
        }

        public async Task<LPanierDto> AddAsync(LPanierDto dto)
        {
            var entity = _mapper.Map<LPanier>(dto);
            await _repository.Add(entity);
            await _repository.Save();

            return _mapper.Map<LPanierDto>(entity);
        }

        public async Task UpdateAsync(LPanierDto dto)
        {
            var entity = _mapper.Map<LPanier>(dto);
            await _repository.Update(entity);
            await _repository.Save();
        }

        public async Task DeleteAsync(params object[] keyValues)
        {
            var entity = await _repository.GetFirstOrDefault(
                predicate: x => x.IdPan == (int)keyValues[0] 
                             && x.IdArt == (int)keyValues[1]
            );

            if (entity != null)
            {
                await _repository.Delete(entity);
                await _repository.Save();
            }
        }
    }
}