using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Records;

public class GetTotalBalanceEndpoint : IEndpoint
{
    public record Query(string UserId) : IRequest<decimal>;

    public class QueryHandler : IRequestHandler<Query, decimal>
    {
        private readonly BudgetDbContext _dbContext;

        public QueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<decimal> Handle(Query query, CancellationToken cancellationToken)
        {
            var totalBalance = await _dbContext.Accounts
                .AsNoTracking()
                .Where(a => a.UserId == query.UserId)
                .SumAsync(a => a.InitialBalance + a.Records.Sum(r => r.Amount), cancellationToken);

            return totalBalance;
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapGet("/records/totalbalance", async (
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var query = new Query(currentUser.Id);
                var result = await mediator.Send(query);

                return Results.Ok(result);
            })
            .RequireAuthorization()
            .WithTags("Records");
    }
}
