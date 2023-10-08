using Budget.Domain.Constants;
using Budget.Domain.Entities;
using Budget.Domain.Exceptions;
using Budget.Domain.Guards;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Accounts;
using Mapster;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRepository<Currency> _currencyRepository;

        public AccountService(IAccountRepository accountRepository, IRepository<Currency> currencyRepository)
        {
            _accountRepository = accountRepository;
            _currencyRepository = currencyRepository;
        }

        public async Task<AccountModel> GetByIdAsync(int accountId, string userId)
        {
            var account = await _accountRepository.GetAccountModelByIdWithCurrencyAsync(accountId, userId);

            if (account == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(account)));
            }

            return account;
        }

        public async Task<IEnumerable<AccountModel>> GetAllAccountsAsync(string userId)
        {
            var accounts = await _accountRepository.GetAllAccountModelsByUserIdAsync(userId);

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

            var account = (createAccountModel, userId).Adapt<Account>();

            var createdAccount = await _accountRepository.CreateAsync(account);

            return createdAccount.Adapt<AccountModel>();
        }

        public async Task<AccountModel> UpdateAsync(UpdateAccountModel accountModel, string userId)
        {
            Guard.IsNotNullOrEmpty(accountModel.Name, nameof(accountModel.Name));
            Guard.ValidateMaxtLength(accountModel.Name, nameof(accountModel.Name), Validations.Accounts.NameMaxLength);

            var account = await _accountRepository.GetByIdWithCurrencyAsync(accountModel.Id, userId);
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

            // TODO: Mapster
            account.CurrencyId = currency.Id;
            account.Name = accountModel.Name;
            account.InitialBalance = accountModel.InitialBalance;

            var updatedAccount = await _accountRepository.UpdateAsync(account);

            return updatedAccount.Adapt<AccountModel>();
        }

        public async Task<AccountModel> DeleteAccountAsync(int accountId, string userId)
        {
            var account = await _accountRepository.GetByIdWithCurrencyAsync(accountId, userId);
            if (account == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(account)));
            }

            if (account.Records.Any())
            {
                throw new BudgetValidationException(ValidationMessages.Accounts.ThereAreRecordsInTheAccount);
            }

            await _accountRepository.DeleteAsync(account);

            return account.Adapt<AccountModel>();
        }
    }
}
