using Budget.Core.Models.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<CategoryModel> GetByIdAsync(int categoryId, string userId);

        Task<IEnumerable<CategoryModel>> GetAllAsync(string userId);

        Task<IEnumerable<CategoryModel>> GetAllPrimaryAsync(string userId);

        Task<IEnumerable<CategoryModel>> GetAllSubcategoriesByParentCategoryIdAsync(int parentCategoryId, string userId);

        Task<CategoryModel> CreateAsync(CreateCategoryModel createCategoryModel, string userId);

        Task<CategoryModel> DeleteAsync(int categoryId, string userId);

        Task<CategoryModel> UpdateAsync(UpdateCategoryModel updateCategoryModel, string userId);
    }
}
