using AutoMapper;
using Core.Entities;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;
using Service.DTO;
using Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRepositoryAsync<Role> _repository;
        private readonly IMapper _mapper;

        public RoleService(
            IRepositoryAsync<Role> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<IEnumerable<RolesDto>> GetAllAsync()
        {
            var entities = await _repository.GetMuliple(
                include: q => q.Include(r => r.IdprofileNavigation)
                               .Include(r => r.IdroleparentNavigation)
                               .Include(r => r.InverseIdroleparentNavigation)
            );

            return _mapper.Map<IEnumerable<RolesDto>>(entities);
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<RolesDto> GetByIdAsync(params object[] keyValues)
        {
            var entity = await _repository.GetFirstOrDefault(
                predicate: r => r.Idrole == (int)keyValues[0],
                include: q => q.Include(r => r.IdprofileNavigation)
                               .Include(r => r.IdroleparentNavigation)
                               .Include(r => r.InverseIdroleparentNavigation)
            );

            return _mapper.Map<RolesDto>(entity);
        }

        // =========================
        // ADD
        // =========================
        public async Task<RolesDto> AddAsync(RolesDto dto)
        {
            var entity = _mapper.Map<Role>(dto);
            await _repository.Add(entity);

            // Recharger l'entité avec les relations
            var addedEntity = await _repository.GetFirstOrDefault(
                predicate: r => r.Idrole == entity.Idrole,
                include: q => q.Include(r => r.IdprofileNavigation)
                               .Include(r => r.IdroleparentNavigation)
                               .Include(r => r.InverseIdroleparentNavigation)
            );

            return _mapper.Map<RolesDto>(addedEntity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task UpdateAsync(RolesDto dto)
        {
            var entity = _mapper.Map<Role>(dto);
            await _repository.Update(entity);
        }

        // =========================
        // DELETE
        // =========================
        public async Task DeleteAsync(params object[] keyValues)
        {
            await _repository.Delete(keyValues);
        }
    }
}