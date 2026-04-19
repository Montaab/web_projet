using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Core.Entities;
using DAL.IRepository;
using Service_ERP.DTO;
using Service_ERP.IService;
using Microsoft.EntityFrameworkCore;

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
            var entities = await _repository.GetAll()
                .Include(f => f.IdComNavigation)
                    .ThenInclude(c => c.LCommandes)
                .ToListAsync();

            return _mapper.Map<IEnumerable<FactureDto>>(entities);
        }

        public async Task<FactureDto> GetByIdAsync(params object[] keyValues)
        {
            if (keyValues == null || keyValues.Length == 0)
                return null;

            var id = (int)keyValues[0];

            var entity = await _repository.GetAll()
                .Include(f => f.IdComNavigation)
                    .ThenInclude(c => c.LCommandes)
                .FirstOrDefaultAsync(f => f.IdFact == id);

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