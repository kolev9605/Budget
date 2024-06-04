
using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Accounts;
using Budget.Application.Accounts;

namespace Budget.Persistance.Repositories;

public class AccountRepository : Repository<Account>, IAccountRepository
{
    public AccountRepository(BudgetDbContext dbContext)
      : base(dbContext)
    {

    }

    public async Task<IEnumerable<Account>> GetAllByUserIdAsync(string userId)
    {
        return await GetAllByUserIdBaseQuery(userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<AccountModel>> GetAllAccountModelsByUserIdAsync(string userId)
    {
        return await GetAllByUserIdBaseQuery(userId)
            .ProjectToType<AccountModel>()
            .ToListAsync();
    }

    public async Task<Account?> GetByIdWithCurrencyAsync(int accountId, string userId)
    {
        var account = await GetByIdWithCurrencyBaseQuery(userId, accountId)
            .FirstOrDefaultAsync();

        return account;
    }

    public async Task<AccountModel?> GetAccountModelByIdWithCurrencyAsync(int accountId, string userId)
    {
        var account = await GetByIdWithCurrencyBaseQuery(userId, accountId)
            .ProjectToType<AccountModel>()
            .FirstOrDefaultAsync();

        return account;
    }

    public async Task<Account?> GetByNameAsync(string userId, string accountName)
    {
        var account = await _budgetDbContext.Accounts
            .Where(a => a.UserId == userId)
            .Where(a => a.Name == accountName)
            .FirstOrDefaultAsync();

        return account;
    }

    private IQueryable<Account> GetAllByUserIdBaseQuery(string userId)
    {
        return _budgetDbContext.Accounts
            .Include(a => a.Currency)
            .Include(a => a.Records)
            .Where(a => a.UserId == userId);
    }

    private IQueryable<Account> GetByIdWithCurrencyBaseQuery(string userId, int accountId)
    {
        return _budgetDbContext.Accounts
            .Include(a => a.Currency)
            .Include(a => a.Records)
            .Where(a => a.UserId == userId)
            .Where(a => a.Id == accountId);
    }
}
