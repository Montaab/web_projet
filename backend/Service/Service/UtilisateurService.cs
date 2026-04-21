using AutoMapper;
using BCrypt.Net;
using Core.Entities;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;
using Service.DTO;
using Service.IService;
using Service.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Service
{
    public class UtilisateurService : IUtilisateurService
    {
        private readonly IRepositoryAsync<Utilisateur> _repository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UtilisateurService(
            IRepositoryAsync<Utilisateur> repository,
            IMapper mapper,
            IAuthService authService)
        {
            _repository = repository;
            _mapper = mapper;
            _authService = authService;
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<IEnumerable<UtilisateurDto>> GetAllAsync()
        {
            var entities = await _repository.GetMuliple(
                include: q => q.Include(u => u.IdroleNavigation)
            );

            return _mapper.Map<IEnumerable<UtilisateurDto>>(entities);
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<UtilisateurDto> GetByIdAsync(params object[] keyValues)
        {
            var entity = await _repository.GetFirstOrDefault(
                predicate: u => u.Iduser == (int)keyValues[0],
                include: q => q.Include(u => u.IdroleNavigation)
            );

            return _mapper.Map<UtilisateurDto>(entity);
        }

        // =========================
        // ADD
        // =========================
        public async Task<UtilisateurDto> AddAsync(UtilisateurDto dto)
        {
            // Vérifier email unique
            var existing = await _repository.GetFirstOrDefault(
                predicate: u => u.Email == dto.Email
            );

            if (existing != null)
                return null;

            // Hash password
            dto.Motpass = BCrypt.Net.BCrypt.HashPassword(dto.Motpass);

            var entity = _mapper.Map<Utilisateur>(dto);

            await _repository.Add(entity);
            await _repository.Save();

            return _mapper.Map<UtilisateurDto>(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task UpdateAsync(UtilisateurDto dto)
        {
            var existing = await _repository.GetById(dto.Iduser);

            if (existing == null)
                return;

            // Gestion password
            if (!string.IsNullOrEmpty(dto.Motpass))
            {
                bool valid = BCrypt.Net.BCrypt.Verify(dto.Motpass, existing.Motpass);

                if (!valid)
                    dto.Motpass = BCrypt.Net.BCrypt.HashPassword(dto.Motpass);
            }

            var entity = _mapper.Map<Utilisateur>(dto);

            await _repository.Update(entity);
            await _repository.Save();
        }

        // =========================
        // DELETE
        // =========================
        public async Task DeleteAsync(params object[] keyValues)
        {
            var entity = await _repository.GetById(keyValues);

            if (entity != null)
            {
                await _repository.Delete(entity);
                await _repository.Save();
            }
        }

        // =========================
        // LOGIN
        // =========================
        public async Task<ResponseLogin?> Login(Login login, CancellationToken ct = default)
        {
            var user = await _repository.GetFirstOrDefault(
                predicate: u => u.Username == login.Username,
                include: q => q.Include(u => u.IdroleNavigation)
            );

            if (user == null)
                return null;

            bool valid = BCrypt.Net.BCrypt.Verify(login.Password, user.Motpass);

            if (!valid)
                return null;

            var userDto = _mapper.Map<UtilisateurDto>(user);
            var token = _authService.GenerateAccessToken(userDto);

            return new ResponseLogin
            {
                AccessToken = token.AccessToken,
                Iduser = user.Iduser,
                Nom = user.Nom,
                Email = user.Email,
                Idrole = user.Idrole,
                TokenType = "Bearer",
                ExpireIn = token.ExpireIn
            };
        }
    }
}