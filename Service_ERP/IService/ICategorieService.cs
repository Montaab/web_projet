using System.Collections.Generic;
using System.Threading.Tasks;
using Service_ERP.DTO;

namespace Service_ERP.IService
{
    public interface ICategorieService
    {
        Task<IEnumerable<CategorieDto>> GetAllAsync();
        Task<CategorieDto> GetByIdAsync(params object[] keyValues);
        Task<CategorieDto> AddAsync(CategorieDto dto);
        Task UpdateAsync(CategorieDto dto);
        Task DeleteAsync(params object[] keyValues);
    }
}
