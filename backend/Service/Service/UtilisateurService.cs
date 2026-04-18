using AutoMapper;
using BCrypt.Net;
using Core.Entities;
using DAL;
using DAL.Config;
using DAL.IRepository;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NuGet.Protocol.Plugins;
using Service.DTO;
using Service.IService;
using Service.Models;
//using Service.Modeles;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;


namespace Service.Service
{
    public class UtilisateurService : ServiceAsync<Utilisateur, UtilisateurDto>, IUtilisateurService
    {

        private readonly IRepositoryAsync<Utilisateur> UtilisateurRepository;
        private readonly IServiceAsync<Utilisateur, UtilisateurDto> srvUtilisateur;
        private readonly IMapper mapper;
        private readonly IAuthService authService;
        //private readonly IHttpClientFactory _httpClientFactory;
        //private readonly ServeurSetting _options;





        public UtilisateurService(IRepositoryAsync<Utilisateur> UtilisateurRepository,
             IServiceAsync<Utilisateur, UtilisateurDto> srvUtilisateur,
             IAuthService _authService,
             IMapper mapper)
            : base(UtilisateurRepository, mapper)
        {

            this.UtilisateurRepository = UtilisateurRepository;
            this.srvUtilisateur = srvUtilisateur;
            authService = _authService;
            this.mapper = mapper;



        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private async Task<UtilisateurDto?> GetUserByUsername(string username)
        {
            var usr = await srvUtilisateur.GetFirstOrDefault(
                predicate: (i => i.Username == username),
                include:(p => p.Include(s => s.IdroleNavigation)),
                disableTracking: true
                );

            return usr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<ResponseLogin?> Islogin(Login login, CancellationToken ct = default)
        {
            var usr = await GetUserByUsername(login.Username);
            if (usr == null)
                return null; // user not found

            // Guard: ensure stored password exists
            if (string.IsNullOrEmpty(usr.Motpass))
                return null;

            bool valid;
            try
            {
                valid = BCrypt.Net.BCrypt.Verify(login.Password, usr.Motpass);
            }
            catch
            {
                // If the stored hash is invalid or Verify throws, treat as invalid credentials
                return null;
            }

            if (!valid)
                return null; // invalid password

            // password is valid -> generate token and return populated ResponseLogin
            var tkn = authService.GenerateAccessToken(usr);

            var responseLogin = new ResponseLogin
            {
               AccessToken = tkn.AccessToken,
               Iduser = usr.Iduser,
               Nom = usr.Nom,
               Email = usr.Email,
               Idrole = usr.Idrole,
               TokenType = "Bearer",
               ExpireIn = tkn.ExpireIn,
            };

            return responseLogin;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Utilisateur"></param>
        /// <returns></returns>
        public async Task<bool> AddUtilisateur(UtilisateurDto Utilisateur)
        {
            var usr = await GetUserByEmail(Utilisateur.Email);
            if (usr == null)
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(Utilisateur.Motpass);
                Utilisateur.Motpass = passwordHash;
                await srvUtilisateur.Add(Utilisateur);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<UtilisateurDto> GetUserByEmail(string email)
        {
            var usr = await srvUtilisateur.GetFirstOrDefault(
                predicate: (i => i.Email == email),
                disableTracking: true
                );
            return usr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Utilisateur"></param>
        /// <returns></returns>
        public async Task<bool> UpdUtilisateur(UtilisateurDto utilisateur)
        {
            var usr = await srvUtilisateur.GetById(utilisateur.Iduser);
            if (usr != null)
            {
                if (!String.IsNullOrEmpty(usr.Motpass))
                {
                    bool valid = BCrypt.Net.BCrypt.Verify(utilisateur.Motpass, usr.Motpass);
                    if (!valid)
                    {
                        string passwordHash = BCrypt.Net.BCrypt.HashPassword(utilisateur.Motpass);
                        utilisateur.Motpass = passwordHash;
                    }
                }

                await srvUtilisateur.UpdateDetachedAsync(utilisateur);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        
    }
}