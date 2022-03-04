using Budget.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Persistance.Seeders
{
    public static class CategoriesSeeder
    {
        public static async Task<BudgetDbContext> AddCategoriesAsync(this BudgetDbContext context)
        {
            if (!await context.Categories.AnyAsync())
            {
                context.Add(new Category() { Name = "Groceries" });
                context.Add(new Category() { Name = "Restaurant" });
                context.Add(new Category() { Name = "Fast-food" });
                context.Add(new Category() { Name = "Bar, cafe" });
                context.Add(new Category() { Name = "Shopping" });
                context.Add(new Category() { Name = "Clothes & shoes" });
                context.Add(new Category() { Name = "Medicaments" });
                context.Add(new Category() { Name = "Kids" });
                context.Add(new Category() { Name = "Electronics" });
                context.Add(new Category() { Name = "Pets, animals" });
                context.Add(new Category() { Name = "Home" });
                context.Add(new Category() { Name = "Sports" });
                context.Add(new Category() { Name = "Rent" });
                context.Add(new Category() { Name = "Income" });
                context.Add(new Category() { Name = "Missing" });
            }

            return context;
        }
    }
}
