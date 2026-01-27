using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Domain.Constants;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Currencies;
using Budget.Infrastructure.Persistence;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Currencies;

public class GetAllCurrenciesEndpoint : IEndpoint
{
    public record Query() : IRequest<ErrorOr<IEnumerable<CurrencyModel>>>;

    public class QueryHandler : IRequestHandler<Query, ErrorOr<IEnumerable<CurrencyModel>>>
    {
        private readonly BudgetDbContext _dbContext;
        private readonly ICacheManager _cacheManager;

        public QueryHandler(BudgetDbContext dbContext, ICacheManager cacheManager)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        public async Task<ErrorOr<IEnumerable<CurrencyModel>>> Handle(Query query, CancellationToken cancellationToken)
        {
            return await _cacheManager.GetOrCreateAsync(
                CacheConstants.Currencies.Key,
                CacheConstants.Currencies.ExpirationInSeconds,
                async () =>
                {
                    var currencies = await _dbContext.Currencies
                        .AsNoTracking()
                        .ProjectToType<CurrencyModel>()
                        .ToListAsync(cancellationToken);

                    return currencies.AsEnumerable().ToErrorOr();
                });
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapGet("/currencies", async (
                IMediator mediator) =>
            {
                var query = new Query();
                var result = await mediator.Send(query);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("Currencies");
    }
}
