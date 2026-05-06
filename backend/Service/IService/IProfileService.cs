using Service.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IProfileService
    {
        // =========================
        // CRUD
        // =========================
        Task<IEnumerable<ProfileDto>> GetAllAsync();
        Task<ProfileDto> GetByIdAsync(params object[] keyValues);
        Task<ProfileDto> AddAsync(ProfileDto dto);
        Task UpdateAsync(ProfileDto dto);
        Task DeleteAsync(params object[] keyValues);
    }
}