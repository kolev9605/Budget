using Budget.Core.Entities;
using Budget.Core.Models.Accounts;
using System.Collections.Generic;
using System.Linq;

namespace Budget.Infrastructure.Factories
{
    public static class AccountFactory
    {
        public static AccountModel ToAccountModel(this Account account)
        {
            if (account == null) return null;

            return new AccountModel()
            {
                Id = account.Id,
                Name = account.Name,
                InitialBalance = account.InitialBalance,
                Currency = account.Currency.ToCurrencyModel(),
                Balance = account.Records.Select(r => r.Amount).Sum() + account.InitialBalance
            };
        }

        public static IEnumerable<AccountModel> ToAccountModels(this IEnumerable<Account> accounts)
        {
            if (accounts == null) return Enumerable.Empty<AccountModel>();

            return accounts.Select(a => a.ToAccountModel());
        }

        public static Account ToAccount(this CreateAccountModel createAccountModel, string userId)
        {
            if (createAccountModel == null) return null;

            var account = new Account()
            {
                Name = createAccountModel.Name,
                InitialBalance = createAccountModel.InitialBalance,
                CurrencyId = createAccountModel.CurrencyId,
                UserId = userId,
            };

            return account;
        }
    }
}
