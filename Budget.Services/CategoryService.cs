namespace Budget.Services
{
    using AutoMapper.QueryableExtensions;
    using Budget.Data.Models;
    using Budget.Data.Models.Enums;
    using Budget.Services.Models;
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CategoryService : ICategoryService
    {
        private readonly BudgetDbContext context;

        public CategoryService(
            BudgetDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<CategoryServiceModel>> GetAllPrimaryAsync()
        {
            var primaryCategories = await context.Categories
                .Where(c => c.IsPrimary)
                .ProjectTo<CategoryServiceModel>()
                .ToListAsync();

            return primaryCategories;
        }

        public async Task<IEnumerable<CategoryServiceModel>> GetAllUserCategoriesByTypeAsync(string userId, TransactionType transactionType)
        {
            var userCategoriesIds = this.context.UserCategories
                .Where(uc => uc.UserId == userId)
                .Select(uc => uc.CategoryId);

            var categories = await context.Categories
                .Where(c => userCategoriesIds.Contains(c.Id))
                .Where(c => c.TransactionType == transactionType)
                .ProjectTo<CategoryServiceModel>()
                .ToListAsync();

            return categories;
        }

        public async Task<TransactionType> GetTransactionTypeByCategoryIdAsync(int categoryId)
        {
            var category = await this.context.Categories
                .FindAsync(categoryId);

            return category.TransactionType;                
        }
    }
}
