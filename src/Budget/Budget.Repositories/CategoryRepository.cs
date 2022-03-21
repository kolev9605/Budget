using Budget.Core.Entities;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Models.Categories;
using Budget.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(BudgetDbContext budgetDbContext) 
            : base(budgetDbContext)
        {
        }

        public async Task<IEnumerable<Category>> GetAllWithSubcategoriesAsync()
        {
            var categories = await _budgetDbContext.Categories
                .Include(c => c.SubCategories)
                .OrderBy(c => c.ParentCategoryId ?? c.Id)
                .ThenBy(c => c.Id)
                .ToListAsync();

            return categories;
        }

        public async Task<IEnumerable<Category>> GetAllPrimaryAsync()
        {
            var categories = await _budgetDbContext.Categories
                .Include(c => c.SubCategories)
                .Where(c => !c.ParentCategoryId.HasValue)
                .ToListAsync();

            return categories;
        }

        public async Task<IEnumerable<Category>> GetSubcategoriesByParentCategoryId(int parentCategoryId)
        {
            var subcategories = await _budgetDbContext.Categories
                .Include(c => c.ParentCategory)
                .Where(c => c.ParentCategoryId.HasValue && c.ParentCategoryId == parentCategoryId)
                .ToListAsync();

            return subcategories;
        }
    }
}
