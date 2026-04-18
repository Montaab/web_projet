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
    public class PanierService : IPanierService
    {
        private readonly IRepositoryAsync<Panier> _repository;
        private readonly IMapper _mapper;

        public PanierService(IRepositoryAsync<Panier> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PanierDto>> GetAllAsync()
        {
            var entities = _repository.GetAll().ToList();
            return _mapper.Map<IEnumerable<PanierDto>>(entities);
        }

        public async Task<PanierDto> GetByIdAsync(params object[] keyValues)
        {
            var entity = await _repository.GetById(keyValues);
            return _mapper.Map<PanierDto>(entity);
        }

        public async Task<PanierDto> AddAsync(PanierDto dto)
        {
            var entity = _mapper.Map<Panier>(dto);
            await _repository.Add(entity);
            await _repository.Save();
            return _mapper.Map<PanierDto>(entity);
        }

        public async Task UpdateAsync(PanierDto dto)
        {
            var entity = _mapper.Map<Panier>(dto);
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
