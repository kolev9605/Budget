using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Currencies;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Budget.Persistance.Repositories;

public class CurrencyRepository : Repository<Currency>, ICurrencyRepository
{
    public CurrencyRepository(BudgetDbContext budgetDbContext) : base(budgetDbContext)
    {
    }

    public async Task<IEnumerable<CurrencyModel>> GetAllAsync()
    {
        return await GetAll()
            .ProjectToType<CurrencyModel>()
            .ToListAsync();
    }

    public async Task<CurrencyModel?> GetByIdAsync(Guid id)
    {
        return await GetAll()
            .Where(c => c.Id == id)
            .ProjectToType<CurrencyModel>()
            .FirstOrDefaultAsync();
    }
}
