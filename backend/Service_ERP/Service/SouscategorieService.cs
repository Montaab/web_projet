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
    public class SouscategorieService : ISouscategorieService
    {
        private readonly IRepositoryAsync<Souscategorie> _repository;
        private readonly IMapper _mapper;

        public SouscategorieService(IRepositoryAsync<Souscategorie> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SouscategorieDto>> GetAllAsync()
        {
            var entities = _repository.GetAll().ToList();
            return _mapper.Map<IEnumerable<SouscategorieDto>>(entities);
        }

        public async Task<SouscategorieDto> GetByIdAsync(params object[] keyValues)
        {
            var entity = await _repository.GetById(keyValues);
            return _mapper.Map<SouscategorieDto>(entity);
        }

        public async Task<SouscategorieDto> AddAsync(SouscategorieDto dto)
        {
            var entity = _mapper.Map<Souscategorie>(dto);
            await _repository.Add(entity);
            await _repository.Save();
            return _mapper.Map<SouscategorieDto>(entity);
        }

        public async Task UpdateAsync(SouscategorieDto dto)
        {
            var entity = _mapper.Map<Souscategorie>(dto);
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
