using Budget.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetByIdWithSubcategoriesAsync(int categoryId, string userId);

        Task<IEnumerable<Category>> GetAllWithSubcategoriesAsync(string userId);

        Task<IEnumerable<Category>> GetAllPrimaryAsync(string userId);

        Task<IEnumerable<Category>> GetSubcategoriesByParentCategoryIdAsync(int parentCategoryId, string userId);

        Task<IEnumerable<Category>> GetInitialCategoriesAsync();

        Task<Category> GetByNameAsync(string name);

        Task<Category> GetForDeletion(int categoryId, string userId);

        Task<Category> GetByNameWithUsersAsync(string name);
    }
}
