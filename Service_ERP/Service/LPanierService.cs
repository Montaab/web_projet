using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Core.Entities;
using DAL.IRepository;
using Service_ERP.DTO;
using Service_ERP.IService;

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
            var entities = _repository.GetAll().ToList();
            return _mapper.Map<IEnumerable<LPanierDto>>(entities);
        }

        public async Task<LPanierDto> GetByIdAsync(params object[] keyValues)
        {
            var entity = await _repository.GetById(keyValues);
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
            var entity = await _repository.GetById(keyValues);
            if (entity != null)
            {
                await _repository.Delete(entity);
                await _repository.Save();
            }
        }
    }
}
