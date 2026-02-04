using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Api.Domain.Common.Errors;
using Budget.Api.Domain.Entities;
using Budget.Api.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Records;

public class GetRecordByIdForUpdateEndpoint : IEndpoint
{
    public class Request
    {
        public Guid RecordId { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
        public string? Note { get; set; }
        public DateTimeOffset RecordDate { get; set; }
        public decimal Amount { get; set; }
        public Guid AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public Guid? FromAccountId { get; set; }
        public string? FromAccountName { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public RecordType RecordType { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }

    public record Query(
        Guid RecordId,
        string UserId) : IRequest<ErrorOr<Response>>;

    public class QueryHandler : IRequestHandler<Query, ErrorOr<Response>>
    {
        private readonly BudgetDbContext _dbContext;

        public QueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorOr<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var record = await _dbContext.Records
                .AsNoTracking()
                .Include(r => r.Account)
                .Include(r => r.Category)
                .Include(r => r.FromAccount)
                .Where(r => r.Id == query.RecordId)
                .Where(r => r.Account.UserId == query.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (record is null)
            {
                return Errors.Record.NotFound;
            }

            // Only the positive transfer record should be edited to simplify the update process
            if (record.RecordType == RecordType.Transfer)
            {
                var positiveTransferRecord = await _dbContext.Records
                    .AsNoTracking()
                    .Include(r => r.Account)
                    .Include(r => r.Category)
                    .Include(r => r.FromAccount)
                    .Where(r => r.RecordDate == record.RecordDate)
                    .Where(r => r.CategoryId == record.CategoryId)
                    .Where(r => Math.Abs(r.Amount) == Math.Abs(record.Amount))
                    .Where(r => r.Amount > 0)
                    .Where(r => r.RecordType == RecordType.Transfer)
                    .Select(r => new Response
                    {
                        Id = r.Id,
                        Note = r.Note,
                        RecordDate = r.RecordDate,
                        Amount = r.Amount,
                        AccountId = r.AccountId,
                        AccountName = r.Account.Name,
                        FromAccountId = r.FromAccountId,
                        FromAccountName = r.FromAccount != null ? r.FromAccount.Name : null,
                        CategoryId = r.CategoryId,
                        CategoryName = r.Category.Name,
                        RecordType = r.RecordType,
                        CreatedOn = r.CreatedOn,
                        UpdatedOn = r.UpdatedOn
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (positiveTransferRecord is null)
                {
                    return Errors.Record.NotFound;
                }

                return positiveTransferRecord;
            }

            return new Response
            {
                Id = record.Id,
                Note = record.Note,
                RecordDate = record.RecordDate,
                Amount = record.Amount,
                AccountId = record.AccountId,
                AccountName = record.Account.Name,
                FromAccountId = record.FromAccountId,
                FromAccountName = record.FromAccount?.Name,
                CategoryId = record.CategoryId,
                CategoryName = record.Category.Name,
                RecordType = record.RecordType,
                CreatedOn = record.CreatedOn,
                UpdatedOn = record.UpdatedOn
            };
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapGet("/records/{recordId}/for-update", async (
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
