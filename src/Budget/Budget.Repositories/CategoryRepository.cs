using Budget.Core.Entities;
using Budget.Core.Interfaces.Repositories;
using Budget.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(BudgetDbContext budgetDbContext) 
            : base(budgetDbContext)
        {
        }

        public async Task<Category> GetByIdWithSubcategoriesAsync(int categoryId, string userId)
        {
            var categories = await GetUserCategories(userId)
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            return categories;
        }

        public async Task<IEnumerable<Category>> GetAllWithSubcategoriesAsync(string userId)
        {
            var categories = await GetUserCategories(userId)
                .Include(c => c.SubCategories)
                .OrderBy(c => c.ParentCategoryId ?? c.Id)
                .ThenBy(c => c.Id)
                .ToListAsync();

            return categories;
        }

        public async Task<IEnumerable<Category>> GetAllPrimaryAsync(string userId)
        {
            var categories = await GetUserCategories(userId)
                .Include(c => c.SubCategories)
                .Where(c => !c.ParentCategoryId.HasValue)
                .OrderBy(c => c.ParentCategoryId ?? c.Id)
                .ThenBy(c => c.Id)
                .ToListAsync();

            return categories;
        }

        public async Task<IEnumerable<Category>> GetSubcategoriesByParentCategoryIdAsync(int parentCategoryId, string userId)
        {
            var subcategories = await GetUserCategories(userId)
                .Include(c => c.ParentCategory)
                .Where(c => c.ParentCategoryId.HasValue && c.ParentCategoryId == parentCategoryId)
                .ToListAsync();

            return subcategories;
        }

        public async Task<IEnumerable<Category>> GetInitialCategoriesAsync()
        {
            var categories = await _budgetDbContext.Categories
                .Where(c => c.IsInitial)
                .ToListAsync();

            return categories;
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            var category = await _budgetDbContext.Categories
                .FirstOrDefaultAsync(c => c.Name == name);

            return category;
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
