using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Api.Domain.Common.Errors;
using Budget.Api.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Accounts;

public class UpdateAccountEndpoint : IEndpoint
{
    public class Request
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid CurrencyId { get; set; }
        public Guid PaymentTypeId { get; set; }
        public decimal InitialBalance { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
    }

    public record Command(
        Guid Id,
        string Name,
        Guid CurrencyId,
        Guid PaymentTypeId,
        decimal InitialBalance,
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
            var account = await _dbContext.Accounts
                .Where(a => a.Id == command.Id)
                .Where(a => a.UserId == command.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (account is null)
            {
                return Errors.Account.NotFound;
            }

            var currencyExists = await _dbContext.Currencies
                .AnyAsync(c => c.Id == command.CurrencyId, cancellationToken);

            if (!currencyExists)
            {
                return Errors.Currency.NotFound;
            }

            var paymentTypeExists = await _dbContext.PaymentTypes
                .AnyAsync(p => p.Id == command.PaymentTypeId, cancellationToken);

            if (!paymentTypeExists)
            {
                return Errors.PaymentType.NotFound;
            }

            account.Name = command.Name;
            account.CurrencyId = command.CurrencyId;
            account.PaymentTypeId = command.PaymentTypeId;
            account.InitialBalance = command.InitialBalance;
            account.UpdatedOn = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new Response { Id = account.Id };
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapPut("/accounts", async (
                Request request,
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var command = new Command(
                    request.Id,
                    request.Name,
                    request.CurrencyId,
                    request.PaymentTypeId,
                    request.InitialBalance,
                    currentUser.Id);
                var result = await mediator.Send(command);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("Accounts");
    }
}
