using Budget.Core.Entities;
using Budget.Application.Interfaces.Repositories;
using Budget.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(BudgetDbContext budgetDbContext) 
            : base(budgetDbContext)
        {
        }

        public async Task<Account> GetByIdWithCurrencyAsync(int accountId, string userId)
            => await _budgetDbContext.Accounts
                .Include(a => a.Currency)
                .Include(a => a.Records)
                .Where(a => a.UserId == userId)
                .FirstOrDefaultAsync(a => a.Id == accountId);

        public async Task<IEnumerable<Account>> GetAllByUserIdAsync(string userId)
            => await _budgetDbContext.Accounts
                .Include(a => a.Currency)
                .Include(a => a.Records)
                .Where(a => a.UserId == userId)
                .ToListAsync();

        public async Task<Account> GetByNameAsync(string userId, string accountName)
            => await _budgetDbContext.Accounts
                .Where(a => a.UserId == userId)
                .Where(a => a.Name == accountName)
                .FirstOrDefaultAsync();
    }
}
