namespace Budget.Services
{
    using AutoMapper.QueryableExtensions;
    using Budget.Data.Models;
    using Budget.Data.Models.Enums;
    using Budget.Services.Models;
    using Contracts;
    using Data;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CategoryService : ICategoryService
    {
        private readonly BudgetDbContext context;
        private readonly UserManager<User> userManager;

        public CategoryService(BudgetDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<CategoryInfoServiceModel>> GetAllUserCategoriesByTypeAsync(string userId, TransactionType transactionType)
        {
            var categories = await this.context.UserCategories
                .Where(uc => uc.UserId == userId && uc.Category.TransactionType == transactionType)
                .Select(uc => uc.Category)
                .ProjectTo<CategoryInfoServiceModel>()
                .ToListAsync();

            return categories;
        }

        public async Task<TransactionType> GetTransactionTypeByCategoryIdAsync(int categoryId)
        {
            var category = await this.context.Categories
                .FindAsync(categoryId);

            return category.TransactionType;
        }

        public async Task<int> AddOrGetCategoryAsync(string name, TransactionType type, string rgbColor, string userId = null)
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
                RgbColorValue = rgbColor
            };

            await this.context.Categories.AddAsync(category);

            if (userId != null)
            {
                if (await this.userManager.FindByIdAsync(userId) == null)
                {
                    throw new ArgumentException($"User with id {userId} is not present in the database.", nameof(userId));
                }

                var userCategory = new UserCategory
                {
                    CategoryId = category.Id,
                    UserId = userId
                };

                await this.context.UserCategories.AddAsync(userCategory);
            }

            var result = await this.context.SaveChangesAsync();

            return category.Id;
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

        public async Task<IEnumerable<CategoryInfoServiceModel>> GetAllCategoriesInfo()
        {
            return await this.context.Categories
                .ProjectTo<CategoryInfoServiceModel>()
                .ToListAsync();
        }

        public IEnumerable<string> GetAllCategoryColors()
            => this.context.Categories.Select(c => c.RgbColorValue);
    }
}