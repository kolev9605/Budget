using Budget.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<TResult> GetByIdWithSubcategoriesAsync<TResult>(int categoryId, string userId);

        Task<IEnumerable<TResult>> GetAllWithSubcategoriesAsync<TResult>(string userId);

        Task<IEnumerable<TResult>> GetAllPrimaryAsync<TResult>(string userId);

        Task<IEnumerable<TResult>> GetSubcategoriesByParentCategoryIdAsync<TResult>(int parentCategoryId, string userId);

        Task<IEnumerable<TResult>> GetInitialCategoriesAsync<TResult>();

        Task<TResult> GetByNameAsync<TResult>(string name);

        Task<TResult> GetForDeletionAsync<TResult>(int categoryId, string userId);

        Task<TResult> GetByNameWithUsersAsync<TResult>(string name);
    }
}
