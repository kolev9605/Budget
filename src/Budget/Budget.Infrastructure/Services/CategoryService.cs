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

        public async Task<CategoryModel> CreateAsync(CreateCategoryModel createCategoryModel, string userId)
        {
            var existingCategory = await _categoriesRepository.GetByNameWithUsersAsync(createCategoryModel.Name);

            // If the existing category matches with the one passed in the createCategoryModel, instead of creating a new category, we should add the user to the UserCategories
            if (existingCategory != null &&
                existingCategory.CategoryType == createCategoryModel.CategoryType &&
                existingCategory.ParentCategoryId == createCategoryModel.ParentCategoryId)
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
                if (existingCategory != null)
                {
                    throw new BudgetValidationException(string.Format(ValidationMessages.Categories.AlreadyExist, existingCategory.Name));
                }

                var category = createCategoryModel.ToCategory(userId);

                var createdCategoryModel = (await _categoriesRepository.CreateAsync(category)).ToCategoryModel();

                return createdCategoryModel;
            }
        }

        public async Task<CategoryModel> DeleteAsync(int categoryId, string userId)
        {
            var existingCategory = await _categoriesRepository.GetForDeletion(categoryId, userId);
            if (existingCategory == null)
            {
                throw new BudgetValidationException(ValidationMessages.Categories.AlreadyDoesNotExist);
            }

            if (existingCategory.Records.Any() || existingCategory.SubCategories.Any(sc => sc.Records.Any()))
            {
                throw new BudgetValidationException(string.Format(ValidationMessages.Categories.HasRecords, existingCategory.Name));
            }

            if (existingCategory.SubCategories.Any())
            {
                throw new BudgetValidationException(string.Format(ValidationMessages.Categories.HasSubCategoriesCannotBeDeleted, existingCategory.Name));
            }

            if (existingCategory.Users.Count == 1 && existingCategory.Users.First().UserId == userId)
            {
                var deletedCategory = (await _categoriesRepository.DeleteAsync(categoryId)).ToCategoryModel();

                return deletedCategory;
            }
            else
            {
                var userItem = existingCategory.Users.First(uc => uc.UserId == userId);
                existingCategory.Users.Remove(userItem);

                var updatedCategory = (await _categoriesRepository.UpdateAsync(existingCategory)).ToCategoryModel();

                return updatedCategory;
            }
        }

        public async Task<CategoryModel> UpdateAsync(UpdateCategoryModel updateCategoryModel, string userId)
        {
            var existingCategory = await _categoriesRepository.GetByIdWithSubcategoriesAsync(updateCategoryModel.Id, userId);
            if (existingCategory == null)
            {
                throw new BudgetValidationException(ValidationMessages.Categories.AlreadyDoesNotExist);
            }

            // Made sub-category
            if (updateCategoryModel.ParentCategoryId.HasValue)
            {
                if (existingCategory.SubCategories.Any())
                {
                    throw new BudgetValidationException(string.Format(ValidationMessages.Categories.HasSubCategoriesCannotBecomeSubcategory, existingCategory.Name));
                }
            }

            existingCategory.ParentCategoryId = updateCategoryModel.ParentCategoryId;
            existingCategory.CategoryType = updateCategoryModel.CategoryType;
            existingCategory.Name = updateCategoryModel.Name;

            var updatedCategory = (await _categoriesRepository.UpdateAsync(existingCategory)).ToCategoryModel();

            return updatedCategory;
        }
    }
}
