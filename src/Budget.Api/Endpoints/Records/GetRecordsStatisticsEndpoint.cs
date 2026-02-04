using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Api.Domain.Entities;
using Budget.Api.Infrastructure.Persistence;
using ErrorOr;
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

    public class Response
    {
        public Guid Id { get; set; }
        public DateTimeOffset RecordDate { get; set; }
        public decimal Amount { get; set; }
        public Guid AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public RecordType RecordType { get; set; }
    }

    public record Query(
        DateTimeOffset StartDateRange,
        DateTimeOffset EndDateRange,
        string UserId) : IRequest<ErrorOr<IEnumerable<Response>>>;

    public class QueryHandler : IRequestHandler<Query, ErrorOr<IEnumerable<Response>>>
    {
        private readonly BudgetDbContext _dbContext;

        public QueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorOr<IEnumerable<Response>>> Handle(Query query, CancellationToken cancellationToken)
        {
            var recordsInRange = await _dbContext.Records
                .AsNoTracking()
                .Include(r => r.Account)
                .Include(r => r.Category)
                .Where(r => r.Account.UserId == query.UserId)
                .Where(r => r.RecordDate >= query.StartDateRange.UtcDateTime && r.RecordDate <= query.EndDateRange.UtcDateTime)
                .OrderBy(r => r.RecordDate)
                .Select(r => new Response
                {
                    Id = r.Id,
                    RecordDate = r.RecordDate,
                    Amount = r.Amount,
                    AccountId = r.AccountId,
                    AccountName = r.Account.Name,
                    CategoryId = r.CategoryId,
                    CategoryName = r.Category.Name,
                    RecordType = r.RecordType
                })
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
