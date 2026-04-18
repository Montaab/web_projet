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
    public class LCommandeService : ILCommandeService
    {
        private readonly IRepositoryAsync<LCommande> _repository;
        private readonly IMapper _mapper;

        public LCommandeService(IRepositoryAsync<LCommande> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LCommandeDto>> GetAllAsync()
        {
            var entities = _repository.GetAll().ToList();
            return _mapper.Map<IEnumerable<LCommandeDto>>(entities);
        }

        public async Task<LCommandeDto> GetByIdAsync(params object[] keyValues)
        {
            var entity = await _repository.GetById(keyValues);
            return _mapper.Map<LCommandeDto>(entity);
        }

        public async Task<LCommandeDto> AddAsync(LCommandeDto dto)
        {
            var entity = _mapper.Map<LCommande>(dto);
            await _repository.Add(entity);
            await _repository.Save();
            return _mapper.Map<LCommandeDto>(entity);
        }

        public async Task UpdateAsync(LCommandeDto dto)
        {
            var entity = _mapper.Map<LCommande>(dto);
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
