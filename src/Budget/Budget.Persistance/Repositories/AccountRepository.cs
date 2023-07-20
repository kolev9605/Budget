
using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Budget.Domain.Interfaces.Repositories;

namespace Budget.Persistance.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(BudgetDbContext dbContext)
          : base(dbContext)
        {

        }

        public async Task<IEnumerable<TResult>> GetAllByUserIdAsync<TResult>(string userId)
        {
            return await _budgetDbContext.Accounts
              .Include(a => a.Currency)
              .Include(a => a.Records)
              .Where(a => a.UserId == userId)
              .ProjectToType<TResult>()
              .ToListAsync();
        }

        public async Task<TResult> GetByIdWithCurrencyAsync<TResult>(int accountId, string userId)
        {
            var account = await _budgetDbContext.Accounts
              .Include(a => a.Currency)
              .Include(a => a.Records)
              .Where(a => a.UserId == userId)
              .Where(a => a.Id == accountId)
              .ProjectToType<TResult>()
              .FirstOrDefaultAsync();

            return account;
        }

        public async Task<TResult> GetByNameAsync<TResult>(string userId, string accountName)
        {
            var account = await _budgetDbContext.Accounts
              .Where(a => a.UserId == userId)
              .Where(a => a.Name == accountName)
              .ProjectToType<TResult>()
              .FirstOrDefaultAsync();

            return account;
        }
    }
}