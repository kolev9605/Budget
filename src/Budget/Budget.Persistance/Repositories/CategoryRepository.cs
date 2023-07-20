using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Persistance;
using Budget.Persistance.Repositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Persistance.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(BudgetDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<TResult>> GetAllPrimaryAsync<TResult>(string userId)
        {
            var categories = await GetUserCategories(userId)
                .Include(c => c.SubCategories)
                .Where(c => !c.ParentCategoryId.HasValue)
                .OrderBy(c => c.ParentCategoryId ?? c.Id)
                .ThenBy(c => c.Id)
                .ProjectToType<TResult>()
                .ToListAsync();

            return categories;
        }

        public async Task<IEnumerable<TResult>> GetAllWithSubcategoriesAsync<TResult>(string userId)
        {
            var categories = await GetUserCategories(userId)
                .Include(c => c.SubCategories)
                .OrderBy(c => c.ParentCategoryId ?? c.Id)
                .ThenBy(c => c.Id)
                .ProjectToType<TResult>()
                .ToListAsync();

            return categories;
        }

        public async Task<TResult> GetByIdWithSubcategoriesAsync<TResult>(int categoryId, string userId)
        {
            var categories = await GetUserCategories(userId)
                .Include(c => c.SubCategories)
                .Where(c => c.Id == categoryId)
                .ProjectToType<TResult>()
                .FirstOrDefaultAsync();

            return categories;
        }

        public async Task<TResult> GetByNameAsync<TResult>(string name)
        {
            var category = await _budgetDbContext.Categories
                .Where(c => c.Name == name)
                .ProjectToType<TResult>()
                .FirstOrDefaultAsync();

            return category;
        }

        public async Task<TResult> GetByNameWithUsersAsync<TResult>(string name)
        {
            var category = await _budgetDbContext.Categories
                .Include(c => c.Users)
                .Where(c => c.Name == name)
                .ProjectToType<TResult>()
                .FirstOrDefaultAsync();

            return category;
        }

        public async Task<TResult> GetForDeletionAsync<TResult>(int categoryId, string userId)
        {
            var categories = await GetUserCategories(userId)
                .Include(c => c.SubCategories)
                    .ThenInclude(sc => sc.Records)
                .Include(c => c.Records)
                .Include(c => c.Users)
                .Where(c => c.Id == categoryId)
                .ProjectToType<TResult>()
                .FirstOrDefaultAsync();

            return categories;
        }

        public async Task<IEnumerable<TResult>> GetInitialCategoriesAsync<TResult>()
        {
            var categories = await _budgetDbContext.Categories
                .Where(c => c.IsInitial)
                .ProjectToType<TResult>()
                .ToListAsync();

            return categories;
        }

        public async Task<IEnumerable<TResult>> GetSubcategoriesByParentCategoryIdAsync<TResult>(int parentCategoryId, string userId)
        {
            var subcategories = await GetUserCategories(userId)
                .Include(c => c.ParentCategory)
                .Where(c => c.ParentCategoryId.HasValue && c.ParentCategoryId == parentCategoryId)
                .ProjectToType<TResult>()
                .ToListAsync();

            return subcategories;
        }

        private IQueryable<Category> GetUserCategories(string userId)
        {
            var categories = _budgetDbContext.Categories
                .Include(c => c.Users)
                .Where(c => c.Users.Where(u => u.UserId == userId).Any());

            return categories;
        }
    }
}