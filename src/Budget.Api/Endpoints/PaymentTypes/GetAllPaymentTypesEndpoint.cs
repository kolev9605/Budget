using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Domain.Constants;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.PaymentTypes;
using Budget.Infrastructure.Persistence;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.PaymentTypes;

public class GetAllPaymentTypesEndpoint : IEndpoint
{
    public record Query() : IRequest<ErrorOr<IEnumerable<PaymentTypeModel>>>;

    public class QueryHandler : IRequestHandler<Query, ErrorOr<IEnumerable<PaymentTypeModel>>>
    {
        private readonly BudgetDbContext _dbContext;
        private readonly ICacheManager _cacheManager;

        public QueryHandler(BudgetDbContext dbContext, ICacheManager cacheManager)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        public async Task<ErrorOr<IEnumerable<PaymentTypeModel>>> Handle(Query query, CancellationToken cancellationToken)
        {
            return await _cacheManager.GetOrCreateAsync(
                CacheConstants.PaymentTypes.Key,
                CacheConstants.PaymentTypes.ExpirationInSeconds,
                async () =>
                {
                    var paymentTypes = await _dbContext.PaymentTypes
                        .AsNoTracking()
                        .ProjectToType<PaymentTypeModel>()
                        .ToListAsync(cancellationToken);

                    return paymentTypes.AsEnumerable().ToErrorOr();
                });
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapGet("/paymenttypes", async (
                IMediator mediator) =>
            {
                var query = new Query();
                var result = await mediator.Send(query);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("PaymentTypes");
    }
}
