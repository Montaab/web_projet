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
    public class MenuService : IMenuService
    {
        private readonly IRepositoryAsync<Menu> _repository;
        private readonly IMapper _mapper;

        public MenuService(
            IRepositoryAsync<Menu> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<IEnumerable<MenuDto>> GetAllAsync()
        {
            var entities = await _repository.GetMuliple(
                include: q => q.Include(m => m.Parent)
                               .Include(m => m.InverseParent)
                               .Include(m => m.Idroles)
            );

            return _mapper.Map<IEnumerable<MenuDto>>(entities);
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<MenuDto> GetByIdAsync(params object[] keyValues)
        {
            var entity = await _repository.GetFirstOrDefault(
                predicate: m => m.Idmenu == (int)keyValues[0],
                include: q => q.Include(m => m.Parent)
                               .Include(m => m.InverseParent)
                               .Include(m => m.Idroles)
            );

            return _mapper.Map<MenuDto>(entity);
        }

        // =========================
        // ADD
        // =========================
        public async Task<MenuDto> AddAsync(MenuDto dto)
        {
            var entity = _mapper.Map<Menu>(dto);
            await _repository.Add(entity);

            // Recharger l'entité avec les relations
            var addedEntity = await _repository.GetFirstOrDefault(
                predicate: m => m.Idmenu == entity.Idmenu,
                include: q => q.Include(m => m.Parent)
                               .Include(m => m.InverseParent)
                               .Include(m => m.Idroles)
            );

            return _mapper.Map<MenuDto>(addedEntity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task UpdateAsync(MenuDto dto)
        {
            var entity = _mapper.Map<Menu>(dto);
            await _repository.Update(entity);
        }

        // =========================
        // DELETE
        // =========================
        public async Task DeleteAsync(params object[] keyValues)
        {
            await _repository.Delete(keyValues);
        }

        // =========================
        // GET BY ROLE ID
        // =========================
        public async Task<IEnumerable<MenuDto>> GetByRoleIdAsync(int roleId)
        {
            var entities = await _repository.GetMuliple(
                predicate: m => m.Idroles.Any(r => r.Idrole == roleId),
                include: q => q.Include(m => m.Parent)
                               .Include(m => m.InverseParent)
                               .Include(m => m.Idroles)
            );

            return _mapper.Map<IEnumerable<MenuDto>>(entities);
        }
    }
}