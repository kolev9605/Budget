using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Api.Domain.Constants;
using Budget.Api.Domain.Interfaces.Services;
using Budget.Api.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.PaymentTypes;

public class GetAllPaymentTypesEndpoint : IEndpoint
{
    public class Response
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public record Query() : IRequest<ErrorOr<IEnumerable<Response>>>;

    public class QueryHandler : IRequestHandler<Query, ErrorOr<IEnumerable<Response>>>
    {
        private readonly BudgetDbContext _dbContext;
        private readonly ICacheManager _cacheManager;

        public QueryHandler(BudgetDbContext dbContext, ICacheManager cacheManager)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        public async Task<ErrorOr<IEnumerable<Response>>> Handle(Query query, CancellationToken cancellationToken)
        {
            return await _cacheManager.GetOrCreateAsync(
                CacheConstants.PaymentTypes.Key,
                CacheConstants.PaymentTypes.ExpirationInSeconds,
                async () =>
                {
                    var paymentTypes = await _dbContext.PaymentTypes
                        .AsNoTracking()
                        .Select(p => new Response
                        {
                            Id = p.Id,
                            Name = p.Name
                        })
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
