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
    public class LFournisseurService : ILFournisseurService
    {
        private readonly IRepositoryAsync<LFournisseur> _repository;
        private readonly IMapper _mapper;

        public LFournisseurService(IRepositoryAsync<LFournisseur> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LFournisseurDto>> GetAllAsync()
        {
            var entities = await _repository.GetMuliple(
                include: q => q
                    .Include(x => x.IdArtNavigation)
                    .Include(x => x.IdFourNavigation)
            );

            return _mapper.Map<IEnumerable<LFournisseurDto>>(entities);
        }

        public async Task<LFournisseurDto> GetByIdAsync(params object[] keyValues)
        {
            var entity = await _repository.GetFirstOrDefault(
                predicate: x => x.IdFour == (int)keyValues[0] 
                             && x.IdArt == (int)keyValues[1],
                include: q => q
                    .Include(x => x.IdArtNavigation)
                    .Include(x => x.IdFourNavigation)
            );

            return _mapper.Map<LFournisseurDto>(entity);
        }

        public async Task<LFournisseurDto> AddAsync(LFournisseurDto dto)
        {
            var entity = _mapper.Map<LFournisseur>(dto);
            await _repository.Add(entity);
            await _repository.Save();

            return _mapper.Map<LFournisseurDto>(entity);
        }

        public async Task UpdateAsync(LFournisseurDto dto)
        {
            var entity = _mapper.Map<LFournisseur>(dto);
            await _repository.Update(entity);
            await _repository.Save();
        }

        public async Task DeleteAsync(params object[] keyValues)
        {
            var entity = await _repository.GetFirstOrDefault(
                predicate: x => x.IdFour == (int)keyValues[0] 
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