using System.Collections.Generic;
using System.Threading.Tasks;
using Service_ERP.DTO;

namespace Service_ERP.IService
{
    public interface IFactureService
    {
        Task<IEnumerable<FactureDto>> GetAllAsync();
        Task<FactureDto> GetByIdAsync(params object[] keyValues);
        Task<FactureDto> AddAsync(FactureDto dto);
        Task UpdateAsync(FactureDto dto);
        Task DeleteAsync(params object[] keyValues);
    }
}
