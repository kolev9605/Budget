using Budget.Domain.Models.Currencies;

namespace Budget.Domain.Models.Accounts
{
    public class AccountModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal InitialBalance { get; set; }

        public decimal Balance { get; set; }

        public CurrencyModel Currency { get; set; }
    }
}
