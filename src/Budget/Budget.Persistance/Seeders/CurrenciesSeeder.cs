using Budget.Core.Entities;

namespace Budget.Persistance.Seeders
{
    public static class CurrenciesSeeder
    {
        public static BudgetDbContext AddCurrencies(this BudgetDbContext context)
        {
            if (!context.Currencies.Any())
            {
                context.Add(new Currency() { Name = "Bulgarian lev", Abbreviation = "BGN" });
                context.Add(new Currency() { Name = "European Euro", Abbreviation = "EUR" });
                context.Add(new Currency() { Name = "U.S. Dollar", Abbreviation = "USD" });
            }

            return context;
        }
    }
}
