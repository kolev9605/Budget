using Budget.Domain.Models.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AccountModel> GetByIdAsync(int accountId, string userId);

        Task<IEnumerable<AccountModel>> GetAllAccountsAsync(string userId);

        Task<AccountModel> CreateAccountAsync(CreateAccountModel createAccountModel, string userId);

        Task<AccountModel> UpdateAsync(UpdateAccountModel accountModel, string userId);

        Task<AccountModel> DeleteAccountAsync(int accountId, string userId);

        Task<AccountModel> TestGetByIdAsync(int accountId, string userId);
    }
}
