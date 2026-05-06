using AutoMapper;
using Core.Entities;
using ProfileEntity = Core.Entities.Profile;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;
using Service.DTO;
using Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Service
{
    public class ProfileService : IProfileService
    {
        private readonly IRepositoryAsync<ProfileEntity> _repository;
        private readonly IMapper _mapper;

        public ProfileService(
            IRepositoryAsync<ProfileEntity> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<IEnumerable<ProfileDto>> GetAllAsync()
        {
            var entities = await _repository.GetMuliple(
                include: q => q.Include(p => p.Roles)
            );

            return _mapper.Map<IEnumerable<ProfileDto>>(entities);
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<ProfileDto> GetByIdAsync(params object[] keyValues)
        {
            var entity = await _repository.GetFirstOrDefault(
                predicate: p => p.Idprofil == (int)keyValues[0],
                include: q => q.Include(p => p.Roles)
            );

            return _mapper.Map<ProfileDto>(entity);
        }

        // =========================
        // ADD
        // =========================
        public async Task<ProfileDto> AddAsync(ProfileDto dto)
        {
            var entity = _mapper.Map<ProfileEntity>(dto);
            await _repository.Add(entity);

            // Recharger l'entité avec les relations
            var addedEntity = await _repository.GetFirstOrDefault(
                predicate: p => p.Idprofil == entity.Idprofil,
                include: q => q.Include(p => p.Roles)
            );

            return _mapper.Map<ProfileDto>(addedEntity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task UpdateAsync(ProfileDto dto)
        {
            var entity = _mapper.Map<ProfileEntity>(dto);
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