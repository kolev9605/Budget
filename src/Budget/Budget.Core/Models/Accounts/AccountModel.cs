using Budget.Core.Entities;
using Budget.Core.Models.Currencies;

namespace Budget.Core.Models.Accounts
{
    public class AccountModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CurrencyModel Currency { get; set; }

        public static AccountModel FromAccount(Account account)
        {
            return new AccountModel()
            {
                Id = account.Id,
                Name = account.Name,
                Currency = new CurrencyModel()
                {
                    Id = account.Currency.Id,
                    Name = account.Currency.Name,
                    Abbreviation = account.Currency.Abbreviation,
                }
            };
        }
    }
}
