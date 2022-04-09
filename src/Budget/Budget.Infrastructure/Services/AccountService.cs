using Budget.Core.Entities;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Budget.Core.Guards;
using Budget.Core.Exceptions;
using Budget.Core.Constants;

namespace Budget.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRepository<Currency> _currencyRepository;

        public AccountService(
            IAccountRepository accountRepository, 
            IRepository<Currency> currencyRepository)
        {
            _accountRepository = accountRepository;
            _currencyRepository = currencyRepository;
        }

        public async Task<AccountModel> GetByIdAsync(int accountId)
        {
            var account = await _accountRepository
                .GetByIdWithCurrencyAsync(accountId);

            var accountModel = AccountModel.FromAccount(account);

            return accountModel;
        }

        public async Task<IEnumerable<AccountModel>> GetAllAccountsAsync(string userId)
        {
            var accounts = await _accountRepository
                .GetAllByUserIdAsync(userId);

            var accountModels = accounts
                .Select(a => AccountModel.FromAccount(a));

            return accountModels;
        }

        public async Task<int> CreateAccountAsync(CreateAccountModel createAccountModel, string userId)
        {
            Guard.IsNotNullOrEmpty(createAccountModel.Name, nameof(createAccountModel.Name));
            Guard.ValidateMaxtLength(createAccountModel.Name, nameof(createAccountModel.Name), Validations.Accounts.NameMaxLength);

            var currency = await _currencyRepository.BaseGetByIdAsync(createAccountModel.CurrencyId);
            if (currency == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(currency)));
            }

            var account = new Account()
            {
                Name = createAccountModel.Name,
                InitialBalance = createAccountModel.InitialBalance,
                CurrencyId = currency.Id,
                UserId = userId,
            };

            var createdAccount = await _accountRepository.CreateAsync(account);

            return createdAccount.Id;
        }

        public async Task<int> UpdateAsync(UpdateAccountModel accountModel)
        {
            Guard.IsNotNullOrEmpty(accountModel.Name, nameof(accountModel.Name));
            Guard.ValidateMaxtLength(accountModel.Name, nameof(accountModel.Name), Validations.Accounts.NameMaxLength);

            var account = await _accountRepository
                .GetByIdWithCurrencyAsync(accountModel.Id);
            if (account == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(account)));
            }

            account.CurrencyId = accountModel.CurrencyId;
            account.Name = accountModel.Name;
            account.InitialBalance = accountModel.InitialBalance;

            await _accountRepository.UpdateAsync(account);

            return account.Id;
        }

        public async Task<int> DeleteAccountAsync(int accountId)
        {
            var account = await _accountRepository.BaseGetByIdAsync(accountId);
            if (account == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(account)));
            }

            var deletedAccount = await _accountRepository.DeleteAsync(accountId);

            return deletedAccount.Id;
        }
    }
}
