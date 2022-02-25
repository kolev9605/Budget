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
        private readonly BudgetDbContext _budgetDbContext;

        public AccountRepository(BudgetDbContext budgetDbContext) 
            : base(budgetDbContext)
        {
            _budgetDbContext = budgetDbContext;
        }

        public async Task<IEnumerable<Account>> GetAllByUserId(string userId)
            => await _budgetDbContext.Accounts
                .Where(a => a.UserId == userId)
                .ToListAsync();
    }
}
