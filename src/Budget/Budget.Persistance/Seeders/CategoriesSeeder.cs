using Budget.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
                    new Category()
                    {
                        Name = "Groceries",
                        CategoryType = CategoryType.Need,
                    },
                    new Category()
                    {
                        Name = "Kids",
                        CategoryType = CategoryType.Need,
                    },
                    new Category()
                    {
                        Name = "Housing",
                        CategoryType = CategoryType.Need,
                        SubCategories= new List<Category>
                        {
                            new Category() { Name = "Bills", CategoryType = CategoryType.Need },
                            new Category() { Name = "Mortgage", CategoryType = CategoryType.Need },
                            new Category() { Name = "Rent", CategoryType = CategoryType.Need },
                            new Category() { Name = "Furniture", CategoryType = CategoryType.Need },
                            new Category() { Name = "Maintenance, repairs", CategoryType = CategoryType.Need },
                            new Category() { Name = "Services", CategoryType = CategoryType.Need },
                            new Category() { Name = "Internet", CategoryType = CategoryType.Need },
                            new Category() { Name = "Phone, mobile phone", CategoryType = CategoryType.Need },
                            new Category() { Name = "New house", CategoryType = CategoryType.Need },
                        }
                    },
                    new Category()
                    {
                        Name = "Car",
                        CategoryType = CategoryType.Need,
                        SubCategories = new List<Category>
                        {
                            new Category() { Name = "Car Maintenance", CategoryType = CategoryType.Need },
                            new Category() { Name = "Fuel", CategoryType = CategoryType.Need },
                            new Category() { Name = "Parking", CategoryType = CategoryType.Need },
                            new Category() { Name = "Car Insurance", CategoryType = CategoryType.Need },
                        }
                    },
                    new Category()
                    {
                        Name = "Healh Care",
                        CategoryType = CategoryType.Need,
                        SubCategories = new List<Category>
                        {
                            new Category() { Name = "Doctor", CategoryType = CategoryType.Need },
                            new Category() { Name = "Medicaments", CategoryType = CategoryType.Need },
                            new Category() { Name = "Dentist", CategoryType = CategoryType.Need },
                        }
                    },
                    new Category()
                    {
                        Name = "Transportation",
                        CategoryType = CategoryType.Need,
                        SubCategories = new List<Category>
                        {
                            new Category() { Name = "Taxi", CategoryType = CategoryType.Need },
                            new Category() { Name = "Public Transport", CategoryType = CategoryType.Need },
                        }
                    },
                    new Category()
                    {
                        Name = "Financial Expenses",
                        CategoryType = CategoryType.Need,
                        SubCategories = new List<Category>
                        {
                            new Category() { Name = "Taxes", CategoryType = CategoryType.Need },
                            new Category() { Name = "Charges & Fees", CategoryType = CategoryType.Need },
                            new Category() { Name = "Fines", CategoryType = CategoryType.Need },
                            new Category() { Name = "Bank fees", CategoryType = CategoryType.Need },
                        }
                    },
                };

                var wants = new List<Category>
                {
                    new Category()
                    {
                        Name = "Eating out",
                        CategoryType = CategoryType.Want,
                        SubCategories = new List<Category>
                        {
                            new Category() { Name = "Fast Food", CategoryType = CategoryType.Want },
                            new Category() { Name = "Resturants", CategoryType = CategoryType.Want },
                            new Category() { Name = "Bar Cafe", CategoryType = CategoryType.Want },
                        }
                    },
                    new Category()
                    {
                        Name = "Shopping",
                        CategoryType= CategoryType.Want,
                        SubCategories = new List<Category>
                        {
                            new Category() { Name = "Clothes", CategoryType = CategoryType.Want },
                            new Category() { Name = "Electronics", CategoryType = CategoryType.Want },
                            new Category() { Name = "Books", CategoryType = CategoryType.Want },
                            new Category() { Name = "Gifts", CategoryType = CategoryType.Want },
                            new Category() { Name = "Partner", CategoryType = CategoryType.Want },
                            new Category() { Name = "Pets", CategoryType = CategoryType.Want },
                            new Category() { Name = "Stationery, tools", CategoryType = CategoryType.Want },
                        }
                    },
                    new Category()
                    {
                        Name = "Life",
                        CategoryType = CategoryType.Want,
                        SubCategories = new List<Category>
                        {
                            new Category() { Name = "Hobbies", CategoryType = CategoryType.Want },
                            new Category() { Name = "Vape", CategoryType = CategoryType.Want },
                            new Category() { Name = "Sports", CategoryType = CategoryType.Want },
                            new Category() { Name = "Education", CategoryType = CategoryType.Want },
                            new Category() { Name = "Online Services", CategoryType = CategoryType.Want },
                            new Category() { Name = "Alcohol, tobacco", CategoryType = CategoryType.Want },
                            new Category() { Name = "Holiday, trips, hotels", CategoryType = CategoryType.Want },
                            new Category() { Name = "Wellness, beauty", CategoryType = CategoryType.Want },
                            new Category() { Name = "Charity", CategoryType = CategoryType.Want },
                            new Category() { Name = "Cinema", CategoryType = CategoryType.Want },
                        }
                    },
                    new Category()
                    {
                        Name = "Investments",
                        CategoryType = CategoryType.Want,
                        SubCategories = new List<Category>
                        {
                            new Category() { Name = "Savings Account", CategoryType = CategoryType.Want },
                        }
                    },
                    new Category()
                    {
                        Name = "Other",
                        CategoryType = CategoryType.Want,
                        SubCategories= new List<Category>
                        {
                            new Category() { Name = "Missing", CategoryType = CategoryType.Want },
                        }
                    },
                };

                var incomes = new List<Category>
                {
                    new Category()
                    {
                        Name = "Income",
                        CategoryType = CategoryType.Income,
                        SubCategories = new List<Category>
                        {
                            new Category() { Name = "Salary", CategoryType = CategoryType.Income },
                            new Category() { Name = "Bank Loan", CategoryType = CategoryType.Income },
                            new Category() { Name = "Refunds (tax, purchase)", CategoryType = CategoryType.Income },
                            new Category() { Name = "Interests, dividends", CategoryType = CategoryType.Income },
                        }
                    },
                };

                var other = new List<Category>
                {
                    new Category()
                    {
                        Name = "Transfer",
                        CategoryType = CategoryType.Transfer,
                    },
                };

                await context.Categories.AddRangeAsync(needs);
                await context.Categories.AddRangeAsync(wants);
                await context.Categories.AddRangeAsync(incomes);
                await context.Categories.AddRangeAsync(other);
            }

            return context;
        }
    }
}
