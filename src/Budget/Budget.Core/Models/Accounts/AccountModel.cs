using Budget.Core.Models.Currencies;

namespace Budget.Core.Models.Accounts
{
    public class AccountModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CurrencyModel Currency { get; set; }
    }
}
