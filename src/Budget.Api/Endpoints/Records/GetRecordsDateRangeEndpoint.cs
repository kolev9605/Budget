using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Api.Domain.Common.Errors;
using Budget.Api.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Records;

public class GetRecordsDateRangeEndpoint : IEndpoint
{
    public class Response
    {
        public DateTimeOffset? MinDate { get; set; }
        public DateTimeOffset? MaxDate { get; set; }
    }

    public record Query(string UserId) : IRequest<ErrorOr<Response>>;

    public class QueryHandler : IRequestHandler<Query, ErrorOr<Response>>
    {
        private readonly BudgetDbContext _dbContext;

        public QueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorOr<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Records
                .AsNoTracking()
                .Include(r => r.Account)
                .Where(r => r.Account.UserId == query.UserId)
                .GroupBy(_ => 1)
                .Select(g => new Response
                {
                    MinDate = g.Min(r => (DateTimeOffset?)r.RecordDate),
                    MaxDate = g.Max(r => (DateTimeOffset?)r.RecordDate)
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (result is null || (result.MinDate is null && result.MaxDate is null))
            {
                return Errors.Record.NoRecords;
            }

            return result;
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapGet("/records/date-range", async (
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var query = new Query(currentUser.Id);
                var result = await mediator.Send(query);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("Records");
    }
}
