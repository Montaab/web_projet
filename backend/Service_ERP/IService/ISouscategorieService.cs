using System.Collections.Generic;
using System.Threading.Tasks;
using Service_ERP.DTO;

namespace Service_ERP.IService
{
    public interface ISouscategorieService
    {
        Task<IEnumerable<SouscategorieDto>> GetAllAsync();
        Task<SouscategorieDto> GetByIdAsync(params object[] keyValues);
        Task<SouscategorieDto> AddAsync(SouscategorieDto dto);
        Task UpdateAsync(SouscategorieDto dto);
        Task DeleteAsync(params object[] keyValues);
    }
}
