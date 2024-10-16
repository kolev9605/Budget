using Budget.Domain.Models.Accounts;

namespace Budget.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AccountModel> GetByIdAsync(Guid accountId, string userId);

        Task<IEnumerable<AccountModel>> GetAllAccountsAsync(string userId);

        Task<AccountModel> CreateAccountAsync(CreateAccountModel createAccountModel, string userId);

        Task<AccountModel> UpdateAsync(UpdateAccountModel accountModel, string userId);

        Task<AccountModel> DeleteAccountAsync(Guid accountId, string userId);
    }
}
