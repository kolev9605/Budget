using Budget.Core.Models.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryModel>> GetAllAsync(string userId);

        Task<IEnumerable<CategoryModel>> GetAllPrimaryAsync(string userId);

        Task<IEnumerable<CategoryModel>> GetAllSubcategoriesByParentCategoryId(int parentCategoryId, string userId);
    }
}
