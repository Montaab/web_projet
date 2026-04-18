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
    public class CategorieService : ICategorieService
    {
        private readonly IRepositoryAsync<Categorie> _repository;
        private readonly IMapper _mapper;

        public CategorieService(IRepositoryAsync<Categorie> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategorieDto>> GetAllAsync()
        {
            var entities = _repository.GetAll().ToList();
            return _mapper.Map<IEnumerable<CategorieDto>>(entities);
        }

        public async Task<CategorieDto> GetByIdAsync(params object[] keyValues)
        {
            var entity = await _repository.GetById(keyValues);
            return _mapper.Map<CategorieDto>(entity);
        }

        public async Task<CategorieDto> AddAsync(CategorieDto dto)
        {
            var entity = _mapper.Map<Categorie>(dto);
            await _repository.Add(entity);
            await _repository.Save();
            return _mapper.Map<CategorieDto>(entity);
        }

        public async Task UpdateAsync(CategorieDto dto)
        {
            var entity = _mapper.Map<Categorie>(dto);
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
