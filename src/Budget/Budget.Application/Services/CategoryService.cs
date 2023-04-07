using Budget.Application.Interfaces;
using Budget.Application.Interfaces.Services;
using Budget.Application.Models.Categories;
using Budget.Domain.Constants;
using Budget.Domain.Entities;
using Budget.Domain.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IBudgetDbContext _budgetDbContext;

        public CategoryService(IBudgetDbContext budgetDbContext)
        {
            _budgetDbContext = budgetDbContext;
        }

        public async Task<CategoryModel> GetByIdAsync(int categoryId, string userId)
        {
            var category = await _budgetDbContext.Categories
                .Include(c => c.Users)
                .Include(c => c.SubCategories)
                .Where(c => c.Users.Any(u => u.UserId == userId))
                .AsNoTracking()
                .ProjectToType<CategoryModel>()
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(category)));
            }

            return category;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllAsync(string userId)
        {
            var categories = await _budgetDbContext.Categories
                .Include(c => c.Users)
                .Include(c => c.SubCategories)
                .Where(c => c.Users.Any(u => u.UserId == userId))
                .OrderBy(c => c.ParentCategoryId ?? c.Id)
                .ThenBy(c => c.Id)
                .AsNoTracking()
                .ProjectToType<CategoryModel>()
                .ToListAsync();

            return categories;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllPrimaryAsync(string userId)
        {
            var categories = await _budgetDbContext.Categories
                .Include(c => c.Users)
                .Include(c => c.SubCategories)
                .Where(c => !c.ParentCategoryId.HasValue)
                .Where(c => c.Users.Any(u => u.UserId == userId))
                .OrderBy(c => c.ParentCategoryId ?? c.Id)
                .ThenBy(c => c.Id)
                .AsNoTracking()
                .ProjectToType<CategoryModel>()
                .ToListAsync();

            return categories;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllSubcategoriesByParentCategoryIdAsync(int parentCategoryId, string userId)
        {
            var categories = await _budgetDbContext.Categories
                .Include(c => c.Users)
                .Include(c => c.ParentCategory)
                .Where(c => c.ParentCategoryId.HasValue && c.ParentCategoryId == parentCategoryId)
                .Where(c => c.Users.Any(u => u.UserId == userId))
                .AsNoTracking()
                .ProjectToType<CategoryModel>()
                .ToListAsync();

            return categories;
        }

        public async Task<CategoryModel> CreateAsync(CreateCategoryModel createCategoryModel, string userId)
        {
            var existingCategory = await _budgetDbContext.Categories
                .Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.Name == createCategoryModel.Name);

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

                var updatedCategory = _budgetDbContext.Categories.Update(existingCategory);
                await _budgetDbContext.SaveChangesAsync();

                return updatedCategory.Entity.Adapt<CategoryModel>();
            }
            else
            {
                if (existingCategory != null)
                {
                    throw new BudgetValidationException(string.Format(ValidationMessages.Categories.AlreadyExist, existingCategory.Name));
                }

                var category = createCategoryModel.Adapt<Category>();
                // TODO: Move this to the Mapster config
                category.Users.Add(new UserCategory()
                {
                    UserId = userId
                });

                var createdCategory = await _budgetDbContext.Categories.AddAsync(category);
                await _budgetDbContext.SaveChangesAsync();

                return createdCategory.Entity.Adapt<CategoryModel>();
            }
        }

        public async Task<CategoryModel> DeleteAsync(int categoryId, string userId)
        {
            var existingCategory = await _budgetDbContext.Categories
                .Include(c => c.Users)
                .Include(c => c.SubCategories)
                    .ThenInclude(sc => sc.Records)
                .Include(c => c.Records)
                .Include(c => c.Users)
                .Where(c => c.Users.Any(u => u.UserId == userId))
                .FirstOrDefaultAsync(c => c.Id == categoryId);

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
                var deletedCategory = _budgetDbContext.Categories.Remove(existingCategory);
                await _budgetDbContext.SaveChangesAsync();

                return deletedCategory.Entity.Adapt<CategoryModel>();
            }
            else
            {
                var userItem = existingCategory.Users.First(uc => uc.UserId == userId);
                existingCategory.Users.Remove(userItem);

                var updatedCategory = _budgetDbContext.Categories.Update(existingCategory);
                await _budgetDbContext.SaveChangesAsync();

                return updatedCategory.Entity.Adapt<CategoryModel>();
            }
        }

        public async Task<CategoryModel> UpdateAsync(UpdateCategoryModel updateCategoryModel, string userId)
        {
            var existingCategory = await _budgetDbContext.Categories
                .Include(c => c.Users)
                .Include(c => c.SubCategories)
                .Where(c => c.Users.Any(u => u.UserId == userId))
                .FirstOrDefaultAsync(c => c.Id == updateCategoryModel.Id);

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

            var updatedCategory = _budgetDbContext.Categories.Update(existingCategory);
            await _budgetDbContext.SaveChangesAsync();

            return updatedCategory.Entity.Adapt<CategoryModel>();
        }
    }
}
