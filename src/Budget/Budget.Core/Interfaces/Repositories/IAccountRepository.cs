using Budget.Core.Entities;
using Budget.Core.Models.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<IEnumerable<Account>> GetAllByUserId(string userId);
    }
}
