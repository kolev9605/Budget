using Budget.Core.Entities;
using Budget.Core.Models.Currencies;
using System.Collections.Generic;
using System.Linq;

namespace Budget.Infrastructure.Factories
{
    public static class CurrencyFactory
    {
        public static CurrencyModel ToCurrencyModel(this Currency currency)
        {
            if (currency == null) return null;

            return new CurrencyModel()
            {
                Id = currency.Id,
                Name = currency.Name,
                Abbreviation = currency.Abbreviation,
            };
        }

        public static IEnumerable<CurrencyModel> ToCurrencyModels(this IEnumerable<Currency> currencies)
        {
            if (currencies == null) return Enumerable.Empty<CurrencyModel>();

            return currencies.Select(c => c.ToCurrencyModel());
        }
    }
}
