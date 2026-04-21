using System.Collections.Generic;
using System.Threading.Tasks;
using Service_ERP.DTO;

namespace Service_ERP.IService
{
    public interface ICommandeService
    {
        Task<IEnumerable<CommandeDto>> GetAllAsync();
        Task<CommandeDto> GetByIdAsync(params object[] keyValues);
        Task<CommandeDto> AddAsync(CommandeDto dto);
        Task UpdateAsync(CommandeDto dto);
        Task DeleteAsync(params object[] keyValues);
    }
}
