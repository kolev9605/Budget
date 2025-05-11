using Budget.Domain.Entities;
using Budget.Domain.Models.Accounts;
using Budget.Domain.Models.Records.Create;

namespace Budget.Domain.Interfaces.Repositories;

public interface IAccountRepository : IRepository<Account>
{
    Task<IEnumerable<AccountModel>> GetAllAccountModelsByUserIdAsync(string userId, bool includeHidden = false);

    Task<Account?> GetByIdWithCurrencyAsync(Guid accountId, string userId);

    Task<AccountModel?> GetAccountModelByIdWithCurrencyAsync(Guid accountId, string userId);

    Task<Account?> GetByNameAsync(string userId, string accountName);

    Task<AccountForRecordCreationModel?> GetForRecordCreationAsync(Guid id);

    Task<decimal> GetTotalBalanceByUserIdAsync(string userId);
}
