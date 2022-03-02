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
                context.Add(new Currency() { Name = "Restaurant" });
                context.Add(new Currency() { Name = "Fast-food" });
                context.Add(new Currency() { Name = "Bar, cafe" });
                context.Add(new Currency() { Name = "Shopping" });
                context.Add(new Currency() { Name = "Clothes & shoes" });
                context.Add(new Currency() { Name = "Medicaments" });
                context.Add(new Currency() { Name = "Kids" });
                context.Add(new Currency() { Name = "Electronics" });
                context.Add(new Currency() { Name = "Pets, animals" });
                context.Add(new Currency() { Name = "Home" });
                context.Add(new Currency() { Name = "Sports" });
                context.Add(new Currency() { Name = "Rent" });
                context.Add(new Currency() { Name = "Income" });
                context.Add(new Currency() { Name = "Missing" });
            }

            return context;
        }
    }
}
