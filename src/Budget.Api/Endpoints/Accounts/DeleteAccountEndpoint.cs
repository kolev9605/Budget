using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Api.Domain.Common.Errors;
using Budget.Api.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Accounts;

public class DeleteAccountEndpoint : IEndpoint
{
    public class Request
    {
        public Guid AccountId { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
    }

    public record Command(
        Guid AccountId,
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
                .Where(a => a.Id == command.AccountId)
                .Where(a => a.UserId == command.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (account is null)
            {
                return Errors.Account.NotFound;
            }

            _dbContext.Accounts.Remove(account);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new Response { Id = account.Id };
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapDelete("/accounts/{accountId}", async (
                [AsParameters] Request request,
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var command = new Command(request.AccountId, currentUser.Id);
                var result = await mediator.Send(command);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("Accounts");
    }
}
