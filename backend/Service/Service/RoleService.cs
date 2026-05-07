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
                               .Include(r => r.Idmenus)
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
                               .Include(r => r.Idmenus)
            );

            return _mapper.Map<RolesDto>(entity);
        }

        // =========================
        // ADD
        // =========================
        public async Task<RolesDto> AddAsync(RolesDto dto)
        {
            var entity = _mapper.Map<Role>(dto);
            
            // IMPORTANT: Récupérer les menus suivis par EF Core pour éviter les conflits
            var db = _repository.DbContextCMC();
            var menuIds = dto.Idmenus?.Select(m => m.Idmenu).ToList() ?? new List<int>();
            
            // Vider la collection générée par AutoMapper (non suivie)
            entity.Idmenus.Clear();
            
            if (menuIds.Any())
            {
                var trackedMenus = await db.Set<Menu>().AsTracking().Where(m => menuIds.Contains(m.Idmenu)).ToListAsync();
                foreach (var menu in trackedMenus)
                {
                    entity.Idmenus.Add(menu);
                }
            }

            await _repository.Add(entity);

            // Recharger l'entité avec les relations
            var addedEntity = await _repository.GetFirstOrDefault(
                predicate: r => r.Idrole == entity.Idrole,
                include: q => q.Include(r => r.IdprofileNavigation)
                               .Include(r => r.IdroleparentNavigation)
                               .Include(r => r.InverseIdroleparentNavigation)
                               .Include(r => r.Idmenus)
            );

            return _mapper.Map<RolesDto>(addedEntity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task UpdateAsync(RolesDto dto)
        {
            var db = _repository.DbContextCMC();
            var existingEntity = await db.Set<Role>()
                .Include(r => r.Idmenus)
                .AsTracking()
                .FirstOrDefaultAsync(r => r.Idrole == dto.Idrole);

            if (existingEntity != null)
            {
                // Mettre à jour les champs scalaires
                existingEntity.Nom = dto.Nom;
                existingEntity.Description = dto.Description;
                existingEntity.Idprofile = dto.Idprofile;
                existingEntity.Idroleparent = dto.Idroleparent;

                // Mettre à jour la relation Many-to-Many
                var menuIds = dto.Idmenus?.Select(m => m.Idmenu).ToList() ?? new List<int>();
                existingEntity.Idmenus.Clear();
                
                if (menuIds.Any())
                {
                    var trackedMenus = await db.Set<Menu>().AsTracking().Where(m => menuIds.Contains(m.Idmenu)).ToListAsync();
                    foreach (var menu in trackedMenus)
                    {
                        existingEntity.Idmenus.Add(menu);
                    }
                }

                await db.SaveChangesAsync();
            }
        }

        // =========================
        // DELETE
        // =========================
        public async Task DeleteAsync(params object[] keyValues)
        {
            if (keyValues != null && keyValues.Length > 0)
            {
                await _repository.Delete(keyValues[0]);
            }
        }
    }
}