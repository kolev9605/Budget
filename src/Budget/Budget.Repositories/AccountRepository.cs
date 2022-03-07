using Budget.Core.Entities;
using Budget.Core.Interfaces.Repositories;
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

        public async Task<Account> GetByIdWithCurrencyAsync(int accountId)
            => await _budgetDbContext.Accounts
                .Include(a => a.Currency)
                .FirstOrDefaultAsync(a => a.Id == accountId);

        public async Task<IEnumerable<Account>> GetAllByUserIdAsync(string userId)
            => await _budgetDbContext.Accounts
                .Include(a => a.Currency)
                .Where(a => a.UserId == userId)
                .ToListAsync();
    }
}
