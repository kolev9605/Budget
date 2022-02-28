using Budget.Core.Models.Currencies;

namespace Budget.Core.Models.Accounts
{
    public class AccountModel
    {
        public string Name { get; set; }

        public CurrencyModel CurrencyModel { get; set; }
    }
}
