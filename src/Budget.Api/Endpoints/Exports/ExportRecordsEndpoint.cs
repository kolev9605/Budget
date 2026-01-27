using System.Text;
using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Budget.Api.Endpoints.Exports;

public class ExportRecordsEndpoint : IEndpoint
{
    public record Query(string UserId) : IRequest<ErrorOr<byte[]>>;

    public class QueryHandler : IRequestHandler<Query, ErrorOr<byte[]>>
    {
        private readonly BudgetDbContext _dbContext;

        public QueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorOr<byte[]>> Handle(Query query, CancellationToken cancellationToken)
        {
            var records = await _dbContext.Records
                .AsNoTracking()
                .Include(r => r.Account)
                .Include(r => r.Category)
                .Where(r => r.Account.UserId == query.UserId)
                .Select(r => new
                {
                    r.Id,
                    r.Note,
                    r.Amount,
                    r.RecordType,
                    r.RecordDate,
                    AccountName = r.Account.Name,
                    CategoryName = r.Category.Name,
                    r.FromAccountId
                })
                .ToListAsync(cancellationToken);

            var result = JsonConvert.SerializeObject(records);
            var bytes = Encoding.UTF8.GetBytes(result);

            return bytes;
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapGet("/exports/records", async (
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var query = new Query(currentUser.Id);
                var result = await mediator.Send(query);

                if (result.IsError)
                {
                    return result.MatchResponse();
                }

                return Results.File(result.Value, "application/json", "records_export.json");
            })
            .RequireAuthorization()
            .WithTags("Exports");
    }
}
