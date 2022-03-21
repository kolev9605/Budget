using Budget.Core.Models.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryModel>> GetAllAsync();

        Task<IEnumerable<CategoryModel>> GetAllPrimaryAsync();

        Task<IEnumerable<CategoryModel>> GetAllSubcategoriesByParentCategoryId(int parentCategoryId);
    }
}
