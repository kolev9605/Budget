using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Domain.Common.Errors;
using Budget.Domain.Models.Records;
using Budget.Infrastructure.Persistence;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Records;

public class GetRecordByIdEndpoint : IEndpoint
{
    public class Request
    {
        public Guid RecordId { get; set; }
    }

    public record Query(
        Guid RecordId,
        string UserId) : IRequest<ErrorOr<RecordModel>>;

    public class QueryHandler : IRequestHandler<Query, ErrorOr<RecordModel>>
    {
        private readonly BudgetDbContext _dbContext;

        public QueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorOr<RecordModel>> Handle(Query query, CancellationToken cancellationToken)
        {
            var record = await _dbContext.Records
                .AsNoTracking()
                .Include(r => r.Account)
                .Include(r => r.Category)
                .Include(r => r.FromAccount)
                .Where(r => r.Id == query.RecordId)
                .Where(r => r.Account.UserId == query.UserId)
                .ProjectToType<RecordModel>()
                .FirstOrDefaultAsync(cancellationToken);

            if (record is null)
            {
                return Errors.Record.NotFound;
            }

            return record;
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapGet("/records/{recordId}", async (
                [AsParameters] Request request,
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var query = new Query(request.RecordId, currentUser.Id);
                var result = await mediator.Send(query);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("Records");
    }
}
