using Budget.Domain.Entities;
using Budget.Domain.Models.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<CategoryModel>> GetAllPrimaryCategoryModelsAsync(string userId);

        Task<IEnumerable<CategoryModel>> GetAllWithSubcategoriesCategoryModelsAsync(string userId);

        Task<Category> GetByIdWithSubcategoriesAsync(int categoryId, string userId);

        Task<CategoryModel> GetByIdWithSubcategoriesMappedAsync(int categoryId, string userId);

        Task<Category> GetByNameWithUsersAsync(string name);

        Task<Category> GetForDeletionAsync(int categoryId, string userId);

        Task<IEnumerable<Category>> GetInitialCategoriesAsync();

        Task<IEnumerable<CategoryModel>> GetSubcategoriesByParentCategoryIdMappedAsync(int parentCategoryId, string userId);
    }
}
