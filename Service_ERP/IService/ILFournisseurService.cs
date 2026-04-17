using System.Collections.Generic;
using System.Threading.Tasks;
using Service_ERP.DTO;

namespace Service_ERP.IService
{
    public interface ILFournisseurService
    {
        Task<IEnumerable<LFournisseurDto>> GetAllAsync();
        Task<LFournisseurDto> GetByIdAsync(params object[] keyValues);
        Task<LFournisseurDto> AddAsync(LFournisseurDto dto);
        Task UpdateAsync(LFournisseurDto dto);
        Task DeleteAsync(params object[] keyValues);
    }
}
