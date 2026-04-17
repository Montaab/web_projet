using System.Collections.Generic;
using System.Threading.Tasks;
using Service_ERP.DTO;

namespace Service_ERP.IService
{
    public interface ILPanierService
    {
        Task<IEnumerable<LPanierDto>> GetAllAsync();
        Task<LPanierDto> GetByIdAsync(params object[] keyValues);
        Task<LPanierDto> AddAsync(LPanierDto dto);
        Task UpdateAsync(LPanierDto dto);
        Task DeleteAsync(params object[] keyValues);
    }
}
