using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Api.Domain.Common.Errors;
using Budget.Api.Domain.Entities;
using Budget.Api.Domain.Interfaces;
using Budget.Api.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Records;

public class CreateRecordEndpoint : IEndpoint
{
    public class Request
    {
        public string? Note { get; set; }
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
        string? Note,
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
            var account = await _dbContext.Accounts
                .Where(a => a.Id == command.AccountId)
                .FirstOrDefaultAsync(cancellationToken);

            if (account is null)
            {
                return Errors.Account.NotFound;
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

            var record = new Record(
                command.Note,
                command.RecordDate.UtcDateTime,
                command.Amount,
                command.AccountId,
                command.FromAccountId,
                command.CategoryId,
                command.RecordType,
                _dateTimeProvider.UtcNowOffset);

            if (record.RecordType == RecordType.Transfer)
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

                var negativeTransferRecord = record.CreateNegativeTransferRecord();
                _dbContext.Records.Add(negativeTransferRecord);

                record.FromAccountId = negativeTransferRecord.AccountId;
            }

            _dbContext.Records.Add(record);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new Response { Id = record.Id };
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapPost("/records", async (
                Request request,
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var command = new Command(
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
