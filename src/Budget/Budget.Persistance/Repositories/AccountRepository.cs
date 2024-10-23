
using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Accounts;
using Budget.Domain.Models.Records.Create;

namespace Budget.Persistance.Repositories;

public class AccountRepository : Repository<Account>, IAccountRepository
{
    public AccountRepository(BudgetDbContext dbContext)
      : base(dbContext)
    {

    }

    // TODO: Projection
    public async Task<IEnumerable<Account>> GetAllByUserIdAsync(string userId)
    {
        return await GetAll()
            .Include(a => a.Currency)
            .Include(a => a.Records)
            .Where(a => a.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<AccountModel>> GetAllAccountModelsByUserIdAsync(string userId)
    {
        return await GetAll()
            .Where(a => a.UserId == userId)
            .ProjectToType<AccountModel>()
            .ToListAsync();
    }

    public async Task<Account?> GetByIdWithCurrencyAsync(Guid accountId, string userId)
    {
        var account = await GetAll()
            .Include(a => a.Currency)
            .Include(a => a.Records)
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
            .Where(a => a.UserId == userId)
            .Where(a => a.Name == accountName)
            .FirstOrDefaultAsync();

        return account;
    }

    public async Task<AccountForRecordCreationModel?> GetForRecordCreationAsync(Guid id)
    {
        return await GetAll()
            .Where(a => a.Id == id)
            .ProjectToType<AccountForRecordCreationModel>()
            .FirstOrDefaultAsync();
    }

    // private IQueryable<Account> GetAllByUserIdBaseQuery(string userId)
    // {
    //     return GetAll()
    //         .Include(a => a.Currency)
    //         .Include(a => a.Records)
    //         .Where(a => a.UserId == userId);
    // }

    // private IQueryable<Account> GetByIdWithCurrencyBaseQuery(string userId, Guid accountId)
    // {
    //     return GetAll()
    //         .Include(a => a.Currency)
    //         .Include(a => a.Records)
    //         .Where(a => a.UserId == userId)
    //         .Where(a => a.Id == accountId);
    // }
}
