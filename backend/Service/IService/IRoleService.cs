using Service.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IRoleService
    {
        // =========================
        // CRUD
        // =========================
        Task<IEnumerable<RolesDto>> GetAllAsync();
        Task<RolesDto> GetByIdAsync(params object[] keyValues);
        Task<RolesDto> AddAsync(RolesDto dto);
        Task UpdateAsync(RolesDto dto);
        Task DeleteAsync(params object[] keyValues);
    }
}