using Budget.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllWithSubcategoriesAsync(string userId);

        Task<IEnumerable<Category>> GetAllPrimaryAsync(string userId);

        Task<IEnumerable<Category>> GetSubcategoriesByParentCategoryId(int parentCategoryId, string userId);

        Task<IEnumerable<Category>> GetInitialCategories();
    }
}
