using Service.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IMenuService
    {
        // =========================
        // CRUD
        // =========================
        Task<IEnumerable<MenuDto>> GetAllAsync();
        Task<MenuDto> GetByIdAsync(params object[] keyValues);
        Task<MenuDto> AddAsync(MenuDto dto);
        Task UpdateAsync(MenuDto dto);
        Task DeleteAsync(params object[] keyValues);
    }
}