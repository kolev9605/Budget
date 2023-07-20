using Budget.Application.Interfaces;
using Budget.Application.Interfaces.Services;
using Budget.Application.Models.Currencies;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Application.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IBudgetDbContext _budgetDbContext;

        public CurrencyService(IBudgetDbContext dbContext)
        {
            _budgetDbContext = dbContext;
        }

        public async Task<IEnumerable<CurrencyModel>> GetAllAsync()
        {
            var currencies = await _budgetDbContext.Currencies
                .ProjectToType<CurrencyModel>()
                .ToListAsync();
                
            return currencies;
        }
    }
}
