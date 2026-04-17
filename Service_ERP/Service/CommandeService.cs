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
    public class CommandeService : ICommandeService
    {
        private readonly IRepositoryAsync<Commande> _repository;
        private readonly IMapper _mapper;

        public CommandeService(IRepositoryAsync<Commande> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommandeDto>> GetAllAsync()
        {
            var entities = _repository.GetAll().ToList();
            return _mapper.Map<IEnumerable<CommandeDto>>(entities);
        }

        public async Task<CommandeDto> GetByIdAsync(params object[] keyValues)
        {
            var entity = await _repository.GetById(keyValues);
            return _mapper.Map<CommandeDto>(entity);
        }

        public async Task<CommandeDto> AddAsync(CommandeDto dto)
        {
            var entity = _mapper.Map<Commande>(dto);
            await _repository.Add(entity);
            await _repository.Save();
            return _mapper.Map<CommandeDto>(entity);
        }

        public async Task UpdateAsync(CommandeDto dto)
        {
            var entity = _mapper.Map<Commande>(dto);
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
