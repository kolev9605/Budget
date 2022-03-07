using Budget.Core.Entities;

namespace Budget.Core.Models.Currencies
{
    public class CurrencyModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public static CurrencyModel FromCurrency(Currency currency)
        {
            return new CurrencyModel()
            {
                Id = currency.Id,
                Name = currency.Name,
                Abbreviation = currency.Abbreviation,
            };
        }
    }
}
