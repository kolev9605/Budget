using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Domain.Entities;
using Budget.Domain.Models.Records.Statistics;
using Budget.Infrastructure.Persistence;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Records;

public class GetRecordsStatisticsEndpoint : IEndpoint
{
    public class Request
    {
        public DateTimeOffset StartDateRange { get; set; }
        public DateTimeOffset EndDateRange { get; set; }
    }

    public record Query(
        DateTimeOffset StartDateRange,
        DateTimeOffset EndDateRange,
        string UserId) : IRequest<ErrorOr<IEnumerable<GetRecordsStatisticsResult>>>;

    public class QueryHandler : IRequestHandler<Query, ErrorOr<IEnumerable<GetRecordsStatisticsResult>>>
    {
        private readonly BudgetDbContext _dbContext;

        public QueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorOr<IEnumerable<GetRecordsStatisticsResult>>> Handle(Query query, CancellationToken cancellationToken)
        {
            var recordsInRange = await _dbContext.Records
                .AsNoTracking()
                .Include(r => r.Account)
                .Include(r => r.Category)
                .Where(r => r.Account.UserId == query.UserId)
                .Where(r => r.RecordDate >= query.StartDateRange.UtcDateTime && r.RecordDate <= query.EndDateRange.UtcDateTime)
                .OrderBy(r => r.RecordDate)
                .ProjectToType<GetRecordsStatisticsResult>()
                .ToListAsync(cancellationToken);

            return recordsInRange.AsEnumerable().ToErrorOr();
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapGet("/records/statistics", async (
                [AsParameters] Request request,
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var query = new Query(
                    request.StartDateRange,
                    request.EndDateRange,
                    currentUser.Id);
                var result = await mediator.Send(query);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("Records");
    }
}
