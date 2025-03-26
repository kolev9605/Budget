using Budget.Application.Currencies.Queries;
using Budget.Domain.Models.Currencies;
using Mapster;

namespace Budget.Tests.Utils.Currencies.Queries;

public static class GetAllCurrenciesQueryMockHelper
{
    // TODO: Accept the arguments
    public static GetAllCurrenciesQueryHandler SetupHandler()
    {
        var currency = EntityMockHelper.SetupCurrency();
        var currencies = new List<CurrencyModel>() { currency.Adapt<CurrencyModel>() };

        var handler = new GetAllCurrenciesQueryHandler(
            RepositoryMockHelper.SetupCurrencyRepository(currency),
            ServiceMockHelper.SetupCacheManager(currencies.AsEnumerable()));

        return handler;
    }

    public static GetAllCurrenciesQuery SetupQuery()
    {
        var command = new GetAllCurrenciesQuery();

        return command;
    }
}
