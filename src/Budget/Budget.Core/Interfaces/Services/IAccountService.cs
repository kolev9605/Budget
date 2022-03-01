using Budget.Core.Models.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface IAccountService
    {
        Task<int> CreateAccountAsync(CreateAccountModel createAccountModel, string userId);

        Task<int> DeleteAccountAsync(int accountId);

        Task<IEnumerable<AccountModel>> GetAllAccountsAsync(string userId);

        Task<AccountModel> GetByIdAsync(int accountId);

        Task<int> UpdateAsync(UpdateAccountModel accountModel);
    }
}
