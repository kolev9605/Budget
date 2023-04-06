using Budget.Application.Interfaces;
using Budget.Application.Interfaces.Services;
using Budget.Application.Models.Accounts;
using Budget.Application.Specifications;
using Budget.Application.Specifications.Accounts;
using Budget.Domain.Constants;
using Budget.Domain.Entities;
using Budget.Domain.Exceptions;
using Budget.Domain.Guards;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IBudgetDbContext _budgetDbContext;

        public AccountService(IBudgetDbContext budgetDbContext)
        {
            _budgetDbContext = budgetDbContext;
        }

        public async Task<AccountModel> GetByIdAsync(int accountId, string userId)
        {
            var account = await _budgetDbContext.Accounts
                .ApplySpecification(new GetAccountByIdWithCurrencySpecification(accountId, userId))
                .ProjectToType<AccountModel>()
                .FirstOrDefaultAsync(a => a.Id == accountId);

            if (account == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(account)));
            }

            return account;
        }

        public async Task<IEnumerable<AccountModel>> GetAllAccountsAsync(string userId)
        {
            var accounts = await _budgetDbContext.Accounts
                .Include(a => a.Currency)
                .Include(a => a.Records)
                .Where(a => a.UserId == userId)
                .ProjectToType<AccountModel>()
                .ToListAsync();

            return accounts;
        }

        public async Task<AccountModel> CreateAccountAsync(CreateAccountModel createAccountModel, string userId)
        {
            Guard.IsNotNullOrEmpty(createAccountModel.Name, nameof(createAccountModel.Name));
            Guard.ValidateMaxtLength(createAccountModel.Name, nameof(createAccountModel.Name), Validations.Accounts.NameMaxLength);

            var currency = await _budgetDbContext.Currencies.FirstOrDefaultAsync(c => c.Id == createAccountModel.CurrencyId);
            if (currency == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(currency)));
            }

            var account = createAccountModel.Adapt<Account>();
            // TODO: Can this be integrated in the Mapster config?
            account.UserId = userId;

            var createdAccount = await _budgetDbContext.Accounts.AddAsync(account);

            await _budgetDbContext.SaveChangesAsync();

            return createdAccount.Entity.Adapt<AccountModel>();
        }

        public async Task<AccountModel> UpdateAsync(UpdateAccountModel accountModel, string userId)
        {
            Guard.IsNotNullOrEmpty(accountModel.Name, nameof(accountModel.Name));
            Guard.ValidateMaxtLength(accountModel.Name, nameof(accountModel.Name), Validations.Accounts.NameMaxLength);

            var account = await _budgetDbContext.Accounts
                .Include(a => a.Currency)
                .Include(a => a.Records)
                .Where(a => a.UserId == userId)
                .FirstOrDefaultAsync(a => a.Id == accountModel.Id);

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

            var currency = await _budgetDbContext.Currencies.FirstOrDefaultAsync(c => c.Id == accountModel.CurrencyId);
            if (currency == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(currency)));
            }

            account.CurrencyId = currency.Id;
            account.Name = accountModel.Name;
            account.InitialBalance = accountModel.InitialBalance;

            var updatedAccount = _budgetDbContext.Accounts.Update(account);

            await _budgetDbContext.SaveChangesAsync();

            return updatedAccount.Entity.Adapt<AccountModel>();
        }

        public async Task<AccountModel> DeleteAccountAsync(int accountId, string userId)
        {
            var account = await _budgetDbContext.Accounts
                .Include(a => a.Currency)
                .Include(a => a.Records)
                .Where(a => a.UserId == userId)
                .FirstOrDefaultAsync(a => a.Id == accountId);

            if (account == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(account)));
            }

            var deletedAccount = _budgetDbContext.Accounts.Remove(account);
            await _budgetDbContext.SaveChangesAsync();

            return deletedAccount.Entity.Adapt<AccountModel>();
        }
    }
}
