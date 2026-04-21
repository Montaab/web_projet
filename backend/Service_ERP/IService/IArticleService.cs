using System.Collections.Generic;
using System.Threading.Tasks;
using Service_ERP.DTO;

namespace Service_ERP.IService
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleDto>> GetAllAsync();
        Task<ArticleDto> GetByIdAsync(params object[] keyValues);
        Task<ArticleDto> AddAsync(ArticleDto dto);
        Task UpdateAsync(ArticleDto dto);
        Task DeleteAsync(params object[] keyValues);
    }
}
