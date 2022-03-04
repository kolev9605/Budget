using Budget.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Persistance.Seeders
{
    public static class CategoriesSeeder
    {
        public static async Task<BudgetDbContext> AddCategoriesAsync(this BudgetDbContext context)
        {
            if (!await context.Categories.AnyAsync())
            {
                var needs = new List<Category>()
                {
                    new Category() { Name = "Groceries" },
                    new Category() { Name = "Clothes" },
                    new Category() { Name = "Medicaments" },
                    new Category() { Name = "Healh Care, Doctor" },
                    new Category() { Name = "Kids" },
                    new Category() { Name = "Home" },
                    new Category() { Name = "Rent" },
                    new Category() { Name = "Mortgage" },
                    new Category() { Name = "Online Services" },
                    new Category() { Name = "Bills" },
                    new Category() { Name = "Sports" },
                    new Category() { Name = "Car Maintenance" },
                }.Select(c => new Category() { Name = c.Name, CategoryType = CategoryType.Need });

                var wants = new List<Category>()
                {
                    new Category() { Name = "Restaurant" },
                    new Category() { Name = "Fast-food" },
                    new Category() { Name = "Bar, cafe" },
                    new Category() { Name = "Shopping" },
                    new Category() { Name = "Electronics" },                    
                    new Category() { Name = "Other" },
                    new Category() { Name = "Hobbies" },
                    new Category() { Name = "Gifts" },
                    new Category() { Name = "Partner" },
                    new Category() { Name = "Travel" },
                }.Select(c => new Category() { Name = c.Name, CategoryType = CategoryType.Want});

                var incomes = new List<Category>()
                {
                    new Category() { Name = "Income" },
                    new Category() { Name = "Salary" },
                }.Select(c => new Category() { Name = c.Name, CategoryType = CategoryType.Income });

                await context.Categories.AddRangeAsync(needs);
                await context.Categories.AddRangeAsync(wants);
                await context.Categories.AddRangeAsync(incomes);
            }

            return context;
        }
    }
}
