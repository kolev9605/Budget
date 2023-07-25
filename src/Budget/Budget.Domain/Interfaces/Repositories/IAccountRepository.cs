using Budget.Domain.Entities;
using Budget.Domain.Models.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<IEnumerable<Account>> GetAllByUserIdAsync(string userId);

        Task<IEnumerable<AccountModel>> GetAllAccountModelsByUserIdAsync(string userId);

        Task<Account> GetByIdWithCurrencyAsync(int accountId, string userId);

        Task<AccountModel> GetAccountModelByIdWithCurrencyAsync(int accountId, string userId);

        Task<Account> GetByNameAsync(string userId, string accountName);
    }
}
