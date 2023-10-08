using Budget.Domain.Constants;
using Budget.Domain.Entities;
using Budget.Domain.Exceptions;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Categories;
using Mapster;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryModel> GetByIdAsync(int categoryId, string userId)
        {
            var category = await _categoryRepository
                .GetByIdWithSubcategoriesMappedAsync(categoryId, userId);

            if (category == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(category)));
            }

            return category;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllAsync(string userId)
        {
            var categories = await _categoryRepository.GetAllWithSubcategoriesCategoryModelsAsync(userId);

            return categories;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllPrimaryAsync(string userId)
        {
            var categories = await _categoryRepository.GetAllPrimaryCategoryModelsAsync(userId);

            return categories;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllSubcategoriesByParentCategoryIdAsync(int parentCategoryId, string userId)
        {
            var categories = await _categoryRepository.GetSubcategoriesByParentCategoryIdMappedAsync(parentCategoryId, userId);

            return categories;
        }

        public async Task<CategoryModel> CreateAsync(CreateCategoryModel createCategoryModel, string userId)
        {
            var existingCategory = await _categoryRepository.GetByNameWithUsersAsync(createCategoryModel.Name);

            // If the existing category matches with the one passed in the createCategoryModel, instead of creating a new category, we should add the user to the UserCategories
            // TODO: Extension method 'ExistsGlobally()'
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

                var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);

                return updatedCategory.Adapt<CategoryModel>();
            }
            else
            {
                if (existingCategory != null)
                {
                    throw new BudgetValidationException(string.Format(ValidationMessages.Categories.AlreadyExist, existingCategory.Name));
                }

                var category = (createCategoryModel, userId).Adapt<Category>();

                var createdCategory = await _categoryRepository.CreateAsync(category);

                return createdCategory.Adapt<CategoryModel>();
            }
        }

        public async Task<CategoryModel> DeleteAsync(int categoryId, string userId)
        {
            var existingCategory = await _categoryRepository.GetForDeletionAsync(categoryId, userId);

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
                var deletedCategory = await _categoryRepository.DeleteAsync(existingCategory);

                return deletedCategory.Adapt<CategoryModel>();
            }
            else
            {
                var userItem = existingCategory.Users.First(uc => uc.UserId == userId);
                existingCategory.Users.Remove(userItem);

                var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);

                return updatedCategory.Adapt<CategoryModel>();
            }
        }

        public async Task<CategoryModel> UpdateAsync(UpdateCategoryModel updateCategoryModel, string userId)
        {
            var existingCategory = await _categoryRepository.GetByIdWithSubcategoriesAsync(updateCategoryModel.Id, userId);
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

            // TODO: Mapster
            existingCategory.ParentCategoryId = updateCategoryModel.ParentCategoryId;
            existingCategory.CategoryType = updateCategoryModel.CategoryType;
            existingCategory.Name = updateCategoryModel.Name;

            var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);

            return updatedCategory.Adapt<CategoryModel>();
        }
    }
}
