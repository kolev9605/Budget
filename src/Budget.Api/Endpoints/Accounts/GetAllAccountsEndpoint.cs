using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Api.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Accounts;

public class GetAllAccountsEndpoint : IEndpoint
{
    public class Request
    {
        public bool? IncludeHidden { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal Balance { get; set; }
        public required CurrencyResponse Currency { get; set; }
        public required PaymentTypeResponse PaymentType { get; set; }
        public bool IsActive { get; set; }

        public class CurrencyResponse
        {
            public Guid Id { get; set; }
            public required string Name { get; set; }
        }

        public class PaymentTypeResponse
        {
            public Guid Id { get; set; }
            public required string Name { get; set; }
        }
    }

    public record Query(
        string UserId,
        bool IncludeHidden) : IRequest<ErrorOr<IEnumerable<Response>>>;

    public class QueryHandler : IRequestHandler<Query, ErrorOr<IEnumerable<Response>>>
    {
        private readonly BudgetDbContext _dbContext;

        public QueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorOr<IEnumerable<Response>>> Handle(Query query, CancellationToken cancellationToken)
        {
            var accounts = await _dbContext.Accounts
                .Where(a => a.UserId == query.UserId)
                .Where(a => query.IncludeHidden || a.IsActive)
                .Select(a => new Response
                {
                    Id = a.Id,
                    Name = a.Name,
                    InitialBalance = a.InitialBalance,
                    Balance = a.InitialBalance + a.Records.Sum(r => r.Amount),
                    Currency = new Response.CurrencyResponse
                    {
                        Id = a.Currency.Id,
                        Name = a.Currency.Name
                    },
                    PaymentType = new Response.PaymentTypeResponse
                    {
                        Id = a.PaymentType.Id,
                        Name = a.PaymentType.Name
                    },
                    IsActive = a.IsActive
                })
                .ToListAsync(cancellationToken);

            return accounts.AsEnumerable().ToErrorOr();
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapGet("/accounts", async (
                [AsParameters] Request request,
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();

                var includeHidden = request.IncludeHidden ?? false;
                var query = new Query(currentUser.Id, includeHidden);
                var result = await mediator.Send(query);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("Accounts");
    }
}

