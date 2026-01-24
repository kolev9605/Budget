using Budget.Api.Helpers;
using Budget.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Accounts;

public record GetAllAccountsResponse(
    Guid Id,
    string Name,
    decimal InitialBalance,
    decimal Balance,
    GetAllAccountsCurrencyResponse Currency,
    GetAllAccountsPaymentTypeResponse PaymentType,
    bool IsActive
);

public record GetAllAccountsCurrencyResponse(
    Guid Id,
    string Name
);

public record GetAllAccountsPaymentTypeResponse(
    Guid Id,
    string Name
);

public static class GetAllAccountsEndpoint
{
    public static void MapGetAllAccountsEndpoint(this WebApplication app)
    {
        app
            .MapGet("/accounts", async (
                [FromQuery] bool includeHidden,
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var query = new GetAllAccountsQuery(currentUser.Id, includeHidden);
                var result = await mediator.Send(query);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("Accounts");
    }
}

public record GetAllAccountsQuery(
    string UserId,
    bool IncludeHidden) : IRequest<ErrorOr<IEnumerable<GetAllAccountsResponse>>>;

public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, ErrorOr<IEnumerable<GetAllAccountsResponse>>>
{
    private readonly BudgetDbContext _dbContext;

    public GetAllAccountsQueryHandler(BudgetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<IEnumerable<GetAllAccountsResponse>>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _dbContext.Accounts
            .Where(a => a.UserId == request.UserId)
            .Where(a => request.IncludeHidden || a.IsActive)
            .Select(a => new GetAllAccountsResponse(
                a.Id,
                a.Name,
                a.InitialBalance,
                a.InitialBalance + a.Records.Sum(r => r.Amount),
                new GetAllAccountsCurrencyResponse(
                    a.Currency.Id,
                    a.Currency.Name),
                new GetAllAccountsPaymentTypeResponse(
                    a.PaymentType.Id,
                    a.PaymentType.Name),
                a.IsActive))
            .ToListAsync(cancellationToken);

        return accounts.AsEnumerable().ToErrorOr();
    }
}
