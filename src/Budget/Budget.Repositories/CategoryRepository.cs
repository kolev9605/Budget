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
                .ToListAsync();

            return categories;
        }
    }
}
