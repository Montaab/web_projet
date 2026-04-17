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
    public class FournisseurService : IFournisseurService
    {
        private readonly IRepositoryAsync<Fournisseur> _repository;
        private readonly IMapper _mapper;

        public FournisseurService(IRepositoryAsync<Fournisseur> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FournisseurDto>> GetAllAsync()
        {
            var entities = _repository.GetAll().ToList();
            return _mapper.Map<IEnumerable<FournisseurDto>>(entities);
        }

        public async Task<FournisseurDto> GetByIdAsync(params object[] keyValues)
        {
            var entity = await _repository.GetById(keyValues);
            return _mapper.Map<FournisseurDto>(entity);
        }

        public async Task<FournisseurDto> AddAsync(FournisseurDto dto)
        {
            var entity = _mapper.Map<Fournisseur>(dto);
            await _repository.Add(entity);
            await _repository.Save();
            return _mapper.Map<FournisseurDto>(entity);
        }

        public async Task UpdateAsync(FournisseurDto dto)
        {
            var entity = _mapper.Map<Fournisseur>(dto);
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
