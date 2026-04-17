using System.Collections.Generic;
using System.Threading.Tasks;
using Service_ERP.DTO;

namespace Service_ERP.IService
{
    public interface ILCommandeService
    {
        Task<IEnumerable<LCommandeDto>> GetAllAsync();
        Task<LCommandeDto> GetByIdAsync(params object[] keyValues);
        Task<LCommandeDto> AddAsync(LCommandeDto dto);
        Task UpdateAsync(LCommandeDto dto);
        Task DeleteAsync(params object[] keyValues);
    }
}
