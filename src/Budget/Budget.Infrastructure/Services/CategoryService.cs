using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Categories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoriesRepository;

        public CategoryService(ICategoryRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<CategoryModel> GetByIdAsync(string userId, int categoryId)
        {
            var category = await _categoriesRepository
                .GetByIdWithSubcategoriesAsync(userId, categoryId);

            var categoryModel = CategoryModel.FromCategory(category);

            return categoryModel;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllAsync(string userId)
        {
            var categories = await _categoriesRepository.GetAllWithSubcategoriesAsync(userId);

            var categoryModels = categories.Select(c => CategoryModel.FromCategory(c));

            return categoryModels;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllPrimaryAsync(string userId)
        {
            var categories = await _categoriesRepository.GetAllPrimaryAsync(userId);

            var categoryModels = categories
                .Select(c => CategoryModel.FromCategory(c));

            return categoryModels;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllSubcategoriesByParentCategoryIdAsync(int parentCategoryId, string userId)
        {
            var categories = await _categoriesRepository.GetSubcategoriesByParentCategoryIdAsync(parentCategoryId, userId);

            var categoryModels = categories
                .Select(c => CategoryModel.FromCategory(c));

            return categoryModels;
        }
    }
}
