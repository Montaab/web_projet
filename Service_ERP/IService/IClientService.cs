using System.Collections.Generic;
using System.Threading.Tasks;
using Service_ERP.DTO;

namespace Service_ERP.IService
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetAllAsync();
        Task<ClientDto> GetByIdAsync(params object[] keyValues);
        Task<ClientDto> AddAsync(ClientDto dto);
        Task UpdateAsync(ClientDto dto);
        Task DeleteAsync(params object[] keyValues);
    }
}
