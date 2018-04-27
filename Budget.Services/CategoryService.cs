namespace Budget.Services
{
    using AutoMapper.QueryableExtensions;
    using Budget.Data.Models;
    using Budget.Data.Models.Enums;
    using Budget.Services.Models;
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CategoryService : ICategoryService
    {
        private readonly BudgetDbContext context;

        public CategoryService(BudgetDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<UserCategoryServiceModel>> GetAllUserCategoriesByTypeAsync(string userId, TransactionType transactionType)
        {
            var userCategoriesIds = this.context.UserCategories
                .Where(uc => uc.UserId == userId)
                .Select(uc => uc.CategoryId);

            var categories = await context.Categories
                .Where(c => userCategoriesIds.Contains(c.Id))
                .Where(c => c.TransactionType == transactionType)
                .ProjectTo<UserCategoryServiceModel>()
                .ToListAsync();

            categories.ForEach(c => c.UserId = userId);

            return categories;
        }

        public async Task<TransactionType> GetTransactionTypeByCategoryIdAsync(int categoryId)
        {
            var category = await this.context.Categories
                .FindAsync(categoryId);

            return category.TransactionType;
        }

        public async Task<int> AddOrGetCategoryAsync(string name, TransactionType type, string rgbColor)
        {
            var existingCategory = this.context.Categories.FirstOrDefault(c => c.Name == name);
            if (existingCategory != null)
            {
                return existingCategory.Id;
            }

            var category = new Category
            {
                Name = name,
                TransactionType = type,
                RgbColorValue = rgbColor,
                IsPrimary = false
            };

            await this.context.Categories.AddAsync(category);
            var result = await this.context.SaveChangesAsync();

            return category.Id;
        }

        public async Task<bool> AddUserCategoryAsync(int categoryId, string userId)
        {
            if (this.context.UserCategories.Where(uc => uc.UserId == userId && uc.CategoryId == categoryId).Any())
            {
                throw new InvalidOperationException($"User category for user with id: {userId} and category with id: {categoryId} already exist.");
            }

            var userCategory = new UserCategory
            {
                CategoryId = categoryId,
                UserId = userId
            };

            await this.context.UserCategories.AddAsync(userCategory);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteUserCategoryAsync(int categoryId, string userId)
        {
            var userCategory = await this.context.UserCategories.FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CategoryId == categoryId);
            if (userCategory == null)
            {
                throw new InvalidOperationException($"User category for user with id: {userId} and category with id: {categoryId} does not exist.");
            }

            if (this.context.Transactions.Any(t => t.CategoryId == userCategory.CategoryId && t.UserId == userId))
            {
                return false;
            }

            this.context.UserCategories.Remove(userCategory);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public IEnumerable<string> GetAllCategoryColors() => this.context.Categories.Select(c => c.RgbColorValue);

        public async Task<IEnumerable<CategoryInfoServiceModel>> GetAllCategoriesInfo()
        {
            return await this.context.Categories
                .ProjectTo<CategoryInfoServiceModel>()
                .ToListAsync();
        }


    }
}
