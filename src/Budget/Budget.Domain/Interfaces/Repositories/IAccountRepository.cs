using Budget.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<IEnumerable<TResult>> GetAllByUserIdAsync<TResult>(string userId);

        Task<TResult> GetByIdWithCurrencyAsync<TResult>(int accountId, string userId);

        Task<TResult> GetByNameAsync<TResult>(string userId, string accountName);
    }
}
