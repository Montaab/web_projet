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
    public class FactureService : IFactureService
    {
        private readonly IRepositoryAsync<Facture> _repository;
        private readonly IMapper _mapper;

        public FactureService(IRepositoryAsync<Facture> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FactureDto>> GetAllAsync()
        {
            var entities = _repository.GetAll().ToList();
            return _mapper.Map<IEnumerable<FactureDto>>(entities);
        }

        public async Task<FactureDto> GetByIdAsync(params object[] keyValues)
        {
            var entity = await _repository.GetById(keyValues);
            return _mapper.Map<FactureDto>(entity);
        }

        public async Task<FactureDto> AddAsync(FactureDto dto)
        {
            var entity = _mapper.Map<Facture>(dto);
            await _repository.Add(entity);
            await _repository.Save();
            return _mapper.Map<FactureDto>(entity);
        }

        public async Task UpdateAsync(FactureDto dto)
        {
            var entity = _mapper.Map<Facture>(dto);
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
