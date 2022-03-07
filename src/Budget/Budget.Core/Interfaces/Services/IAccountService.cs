using Budget.Core.Models.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AccountModel> GetByIdAsync(int accountId);

        Task<IEnumerable<AccountModel>> GetAllAccountsAsync(string userId);

        Task<int> CreateAccountAsync(CreateAccountModel createAccountModel, string userId);

        Task<int> UpdateAsync(UpdateAccountModel accountModel);

        Task<int> DeleteAccountAsync(int accountId);
    }
}
