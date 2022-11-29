using Budget.Core.Constants;
using Budget.Core.Entities;
using Budget.Core.Exceptions;
using Budget.Core.Guards;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Categories;
using Budget.Infrastructure.Factories;
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

        public async Task<CategoryModel> GetByIdAsync(int categoryId, string userId)
        {
            var category = await _categoriesRepository
                .GetByIdWithSubcategoriesAsync(categoryId, userId);

            if (category == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(category)));
            }

            var categoryModel = category.ToCategoryModel();

            return categoryModel;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllAsync(string userId)
        {
            var categories = await _categoriesRepository.GetAllWithSubcategoriesAsync(userId);

            var categoryModels = categories.ToCategoryModels();

            return categoryModels;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllPrimaryAsync(string userId)
        {
            var categories = await _categoriesRepository.GetAllPrimaryAsync(userId);

            var categoryModels = categories.ToCategoryModels();

            return categoryModels;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllSubcategoriesByParentCategoryIdAsync(int parentCategoryId, string userId)
        {
            var categories = await _categoriesRepository.GetSubcategoriesByParentCategoryIdAsync(parentCategoryId, userId);

            var categoryModels = categories.ToCategoryModels();

            return categoryModels;
        }

        public async Task<CategoryModel> CreateAsync(CreateCategoryModel model, string userId)
        {
            var existingCategory = await _categoriesRepository.GetByNameWithUsersAsync(model.Name);
            if (existingCategory != null &&
                existingCategory.CategoryType == model.CategoryType &&
                existingCategory.ParentCategoryId == model.ParentCategoryId)
            {
                if (existingCategory.Users.Any(uc => uc.UserId == userId))
                {
                    throw new BudgetValidationException(string.Format(ValidationMessages.Categories.AlreadyExist, existingCategory.Name));
                }

                existingCategory.Users.Add(new UserCategory
                {
                    UserId = userId
                });

                var updatedCategory = (await _categoriesRepository.UpdateAsync(existingCategory)).ToCategoryModel();

                return updatedCategory;
            }
            else
            {
                var category = model.ToCategory(userId);

                var createdCategoryModel = (await _categoriesRepository.CreateAsync(category)).ToCategoryModel();

                return createdCategoryModel;
            }
        }
    }
}
