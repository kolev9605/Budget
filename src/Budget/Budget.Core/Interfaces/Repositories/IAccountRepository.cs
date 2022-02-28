using Budget.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<IEnumerable<Account>> GetAllByUserIdAsync(string userId);

        Task<Account> GetByIdWithCurrencyAsync(int accountId);
    }
}
