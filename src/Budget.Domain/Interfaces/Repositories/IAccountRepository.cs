using Budget.Domain.Entities;
using Budget.Domain.Models.Accounts;
using Budget.Domain.Models.Records.Create;

namespace Budget.Domain.Interfaces.Repositories;

public interface IAccountRepository : IRepository<Account>
{
    Task<IEnumerable<Account>> GetAllByUserIdAsync(string userId);

    Task<IEnumerable<AccountModel>> GetAllAccountModelsByUserIdAsync(string userId);

    Task<Account?> GetByIdWithCurrencyAsync(Guid accountId, string userId);

    Task<AccountModel?> GetAccountModelByIdWithCurrencyAsync(Guid accountId, string userId);

    Task<Account?> GetByNameAsync(string userId, string accountName);

    Task<AccountForRecordCreationModel?> GetForRecordCreationAsync(Guid id);
}
