using Budget.Domain.Constants;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Currencies;
using ErrorOr;
using MediatR;

namespace Budget.Application.Currencies.Queries;

public record GetAllCurrenciesQuery() : IRequest<ErrorOr<IEnumerable<CurrencyModel>>>;

public class GetAllCurrenciesQueryHandler : IRequestHandler<GetAllCurrenciesQuery, ErrorOr<IEnumerable<CurrencyModel>>>
{
    private readonly ICurrencyRepository _currencyRepository;
    private readonly ICacheManager _cacheManager;

    public GetAllCurrenciesQueryHandler(
        ICurrencyRepository currencyRepository,
        ICacheManager cacheManager)
    {
        _currencyRepository = currencyRepository;
        _cacheManager = cacheManager;
    }

    public async Task<ErrorOr<IEnumerable<CurrencyModel>>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
    {
        // TODO: Caching Mediatr behavior can be implemented
        return (await _cacheManager.GetOrCreateAsync(
            CacheConstants.Keys.Currencies,
            CacheConstants.Expirations.CurrenciesExpirationInSeconds,
            _currencyRepository.GetAllItems)).ToErrorOr();
    }
}
