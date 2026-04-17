using System.Collections.Generic;
using System.Threading.Tasks;
using Service_ERP.DTO;

namespace Service_ERP.IService
{
    public interface IPanierService
    {
        Task<IEnumerable<PanierDto>> GetAllAsync();
        Task<PanierDto> GetByIdAsync(params object[] keyValues);
        Task<PanierDto> AddAsync(PanierDto dto);
        Task UpdateAsync(PanierDto dto);
        Task DeleteAsync(params object[] keyValues);
    }
}
