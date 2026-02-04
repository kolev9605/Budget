using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Api.Domain.Common.Errors;
using Budget.Api.Domain.Entities;
using Budget.Api.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Records;

public class DeleteRecordEndpoint : IEndpoint
{
    public class Request
    {
        public Guid RecordId { get; set; }
    }

    public class Response
    {
    }

    public record Command(
        Guid RecordId,
        string UserId) : IRequest<ErrorOr<Response>>;

    public class CommandHandler : IRequestHandler<Command, ErrorOr<Response>>
    {
        private readonly BudgetDbContext _dbContext;

        public CommandHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorOr<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var record = await _dbContext.Records
                .Include(r => r.Account)
                .Where(r => r.Id == command.RecordId)
                .Where(r => r.Account.UserId == command.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (record is null)
            {
                return Errors.Record.NotFound;
            }

            // Find and delete the paired transfer record if it exists
            if (record.RecordType == RecordType.Transfer)
            {
                var existingTransferRecord = await _dbContext.Records
                    .Include(r => r.Account)
                    .Where(r => r.Account.UserId == command.UserId)
                    .Where(r => r.AccountId == record.FromAccountId)
                    .Where(r => r.FromAccountId == record.AccountId)
                    .Where(r => Math.Abs(r.Amount) == Math.Abs(record.Amount))
                    .Where(r => r.RecordType == RecordType.Transfer)
                    .Where(r => r.CreatedOn == record.CreatedOn)
                    .Where(r => r.CategoryId == record.CategoryId)
                    .Where(r => r.Id != record.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingTransferRecord is not null)
                {
                    _dbContext.Records.Remove(existingTransferRecord);
                }
            }

            _dbContext.Records.Remove(record);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new Response();
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapDelete("/records/{recordId}", async (
                [AsParameters] Request request,
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var command = new Command(request.RecordId, currentUser.Id);
                var result = await mediator.Send(command);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("Records");
    }
}
