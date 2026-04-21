using Service.DTO;
using Service.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IUtilisateurService
    {
        // =========================
        // CRUD
        // =========================
        Task<IEnumerable<UtilisateurDto>> GetAllAsync();
        Task<UtilisateurDto> GetByIdAsync(params object[] keyValues);
        Task<UtilisateurDto> AddAsync(UtilisateurDto dto);
        Task UpdateAsync(UtilisateurDto dto);
        Task DeleteAsync(params object[] keyValues);

        // =========================
        // AUTH
        // =========================
        Task<ResponseLogin?> Login(Login login, CancellationToken ct = default);
    }
}