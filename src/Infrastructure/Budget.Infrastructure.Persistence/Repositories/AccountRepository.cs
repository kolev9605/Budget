
using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Accounts;
using Budget.Domain.Models.Records.Create;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Persistence.Repositories;

public class AccountRepository : Repository<Account>, IAccountRepository
{
    public AccountRepository(BudgetDbContext dbContext)
      : base(dbContext)
    {

    }

    public async Task<IEnumerable<AccountModel>> GetAllAccountModelsByUserIdAsync(string userId, bool includeHidden = false)
    {
        var query = GetAll();

        if (!includeHidden)
        {
            query = query.Where(a => a.IsActive);
        }

        return await query
            .Where(a => a.UserId == userId)
            .ProjectToType<AccountModel>()
            .ToListAsync();
    }

    public async Task<Account?> GetByIdWithCurrencyAsync(Guid accountId, string userId)
    {
        var account = await GetAll()
            .Include(a => a.Currency)
            .Include(a => a.Records)
            .Include(a => a.PaymentType)
            .Where(a => a.UserId == userId)
            .Where(a => a.Id == accountId)
            .FirstOrDefaultAsync();

        return account;
    }

    public async Task<AccountModel?> GetAccountModelByIdWithCurrencyAsync(Guid accountId, string userId)
    {
        var account = await GetAll()
            .Where(a => a.UserId == userId)
            .Where(a => a.Id == accountId)
            .ProjectToType<AccountModel>()
            .FirstOrDefaultAsync();

        return account;
    }

    public async Task<Account?> GetByNameAsync(string userId, string accountName)
    {
        var account = await GetAll()
            .Where(a => a.IsActive)
            .Where(a => a.UserId == userId)
            .Where(a => a.Name == accountName)
            .FirstOrDefaultAsync();

        return account;
    }

    public async Task<AccountForRecordCreationModel?> GetForRecordCreationAsync(Guid id)
    {
        return await GetAll()
            .Where(a => a.IsActive)
            .Where(a => a.Id == id)
            .ProjectToType<AccountForRecordCreationModel>()
            .FirstOrDefaultAsync();
    }

    public async Task<decimal> GetTotalBalanceByUserIdAsync(string userId)
    {
        var totalBalance = await GetAll()
            .Where(a => a.UserId == userId)
            .Select(a => a.InitialBalance + a.Records.Select(r => r.Amount).Sum())
            .SumAsync();

        return totalBalance;
    }
}
