using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces;
using Budget.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Records;

public class UpdateRecordEndpoint : IEndpoint
{
    public class Request
    {
        public Guid RecordId { get; set; }
        public string Note { get; set; } = null!;
        public decimal Amount { get; set; }
        public Guid AccountId { get; set; }
        public Guid CategoryId { get; set; }
        public RecordType RecordType { get; set; }
        public DateTimeOffset RecordDate { get; set; }
        public Guid? FromAccountId { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
    }

    public record Command(
        Guid RecordId,
        string Note,
        decimal Amount,
        Guid AccountId,
        Guid CategoryId,
        RecordType RecordType,
        DateTimeOffset RecordDate,
        Guid? FromAccountId,
        string UserId) : IRequest<ErrorOr<Response>>;

    public class CommandHandler : IRequestHandler<Command, ErrorOr<Response>>
    {
        private readonly BudgetDbContext _dbContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommandHandler(
            BudgetDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
            _userManager = userManager;
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

            var account = await _dbContext.Accounts
                .Where(a => a.Id == command.AccountId)
                .FirstOrDefaultAsync(cancellationToken);

            if (account is null)
            {
                return Errors.Record.NotFound;
            }

            if (account.UserId != command.UserId)
            {
                return Errors.Account.BelongsToAnotherUser;
            }

            var userExists = await _userManager.FindByIdAsync(command.UserId) is not null;
            if (!userExists)
            {
                return Errors.User.NotFound;
            }

            var categoryExists = await _dbContext.Categories
                .AnyAsync(c => c.Id == command.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                return Errors.Category.NotFound;
            }

            record.Update(
                command.Note,
                command.RecordDate,
                command.Amount,
                command.AccountId,
                command.FromAccountId,
                command.CategoryId,
                command.RecordType,
                _dateTimeProvider.UtcNow);

            if (command.RecordType == RecordType.Transfer)
            {
                if (!command.FromAccountId.HasValue)
                {
                    return Errors.Account.NotFound;
                }

                var fromAccount = await _dbContext.Accounts
                    .Where(a => a.Id == command.FromAccountId.Value)
                    .FirstOrDefaultAsync(cancellationToken);

                if (fromAccount is null)
                {
                    return Errors.Account.NotFound;
                }

                if (account.Id == fromAccount.Id)
                {
                    return Errors.Record.SameAccountsInTransfer;
                }

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

                if (existingTransferRecord is null)
                {
                    return Errors.Record.NotFound;
                }

                existingTransferRecord.Update(
                    command.Note,
                    command.RecordDate,
                    command.Amount,
                    accountId: command.FromAccountId.Value,
                    fromAccountId: record.AccountId,
                    command.CategoryId,
                    command.RecordType,
                    _dateTimeProvider.UtcNow,
                    true);

                record.FromAccountId = existingTransferRecord.AccountId;
            }
            else
            {
                record.FromAccountId = null;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new Response { Id = record.Id };
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapPut("/records/{recordId}", async (
                Guid recordId,
                Request request,
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var command = new Command(
                    recordId,
                    request.Note,
                    request.Amount,
                    request.AccountId,
                    request.CategoryId,
                    request.RecordType,
                    request.RecordDate,
                    request.FromAccountId,
                    currentUser.Id);
                var result = await mediator.Send(command);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("Records");
    }
}
