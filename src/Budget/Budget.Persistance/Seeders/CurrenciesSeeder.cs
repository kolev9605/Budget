﻿using Budget.Core.Entities;
using Microsoft.EntityFrameworkCore;

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