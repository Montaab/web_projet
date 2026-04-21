using System.Collections.Generic;
using System.Threading.Tasks;
using Service_ERP.DTO;

namespace Service_ERP.IService
{
    public interface IFournisseurService
    {
        Task<IEnumerable<FournisseurDto>> GetAllAsync();
        Task<FournisseurDto> GetByIdAsync(params object[] keyValues);
        Task<FournisseurDto> AddAsync(FournisseurDto dto);
        Task UpdateAsync(FournisseurDto dto);
        Task DeleteAsync(params object[] keyValues);
    }
}
