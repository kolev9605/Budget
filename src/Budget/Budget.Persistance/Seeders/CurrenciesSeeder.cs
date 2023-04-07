using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Budget.Persistance.Seeders
{
    public static class CurrenciesSeeder
    {
        public static async Task<BudgetDbContext> AddCurrenciesAsync(this BudgetDbContext context)
        {
            if (!await context.Currencies.AnyAsync())
            {
                context.Add(new Currency() { Name = "Bulgarian lev", Abbreviation = "BGN" });
                context.Add(new Currency() { Name = "European Euro", Abbreviation = "EUR" });
                context.Add(new Currency() { Name = "U.S. Dollar", Abbreviation = "USD" });
            }

            return context;
        }
    }
}
