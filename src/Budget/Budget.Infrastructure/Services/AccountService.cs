using Budget.Core.Entities;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Budget.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<int> CreateAccountAsync(CreateAccountModel createAccountModel, string userId)
        {
            var account = new Account()
            {
                Name = createAccountModel.Name,
                CurrencyId = createAccountModel.CurrencyId,
                UserId = userId,
            };

            var createdAccount = await _accountRepository.CreateAsync(account);

            return createdAccount.Id;
        }

        public async Task<IEnumerable<AccountModel>> GetAllAccountsAsync(string userId)
        {
            var accounts = await _accountRepository
                .GetAllByUserId(userId);

            var accountModels = accounts
                .Select(a => new AccountModel()
                {
                    Name = a.Name,
                });

            return accountModels;
        }
    }
}
