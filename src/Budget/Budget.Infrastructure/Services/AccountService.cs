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
using Budget.Infrastructure.Factories;

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

        public async Task<AccountModel> GetByIdAsync(int accountId, string userId)
        {
            var account = await _accountRepository
                .GetByIdWithCurrencyAsync(accountId, userId);

            if (account == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(account)));
            }

            var accountModel = account.ToAccountModel();

            return accountModel;
        }

        public async Task<IEnumerable<AccountModel>> GetAllAccountsAsync(string userId)
        {
            var accounts = (await _accountRepository
                .GetAllByUserIdAsync(userId))
                .ToAccountModels();

            return accounts;
        }

        public async Task<AccountModel> CreateAccountAsync(CreateAccountModel createAccountModel, string userId)
        {
            Guard.IsNotNullOrEmpty(createAccountModel.Name, nameof(createAccountModel.Name));
            Guard.ValidateMaxtLength(createAccountModel.Name, nameof(createAccountModel.Name), Validations.Accounts.NameMaxLength);

            var currency = await _currencyRepository.BaseGetByIdAsync(createAccountModel.CurrencyId);
            if (currency == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(currency)));
            }

            var account = createAccountModel.ToAccount(userId);

            var createdAccount = (await _accountRepository.CreateAsync(account)).ToAccountModel();

            return createdAccount;
        }

        public async Task<AccountModel> UpdateAsync(UpdateAccountModel accountModel, string userId)
        {
            Guard.IsNotNullOrEmpty(accountModel.Name, nameof(accountModel.Name));
            Guard.ValidateMaxtLength(accountModel.Name, nameof(accountModel.Name), Validations.Accounts.NameMaxLength);

            var account = await _accountRepository
                .GetByIdWithCurrencyAsync(accountModel.Id, userId);
            if (account == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(account)));
            }

            if (account.UserId != userId)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Accounts.InvalidAccount, account.Name));
            }

            var currency = await _currencyRepository.BaseGetByIdAsync(accountModel.CurrencyId);
            if (currency == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(currency)));
            }

            account.CurrencyId = currency.Id;
            account.Name = accountModel.Name;
            account.InitialBalance = accountModel.InitialBalance;

            var updatedAccount = (await _accountRepository.UpdateAsync(account)).ToAccountModel();

            return updatedAccount;
        }

        public async Task<AccountModel> DeleteAccountAsync(int accountId, string userId)
        {
            var account = await _accountRepository.GetByIdWithCurrencyAsync(accountId, userId);
            if (account == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(account)));
            }

            var deletedAccount = (await _accountRepository.DeleteAsync(accountId)).ToAccountModel();

            return deletedAccount;
        }
    }
}
