using Budget.Api.Helpers;
using Budget.Api.Models;
using Budget.Domain.Common.Errors;
using Budget.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Accounts;

public record GetAccountByIdResponse(
    Guid Id,
    string Name,
    decimal InitialBalance,
    decimal Balance,
    GetAccountByIdCurrencyResponse Currency,
    GetAccountByIdPaymentTypeResponse PaymentType,
    bool IsActive
);

public record GetAccountByIdCurrencyResponse(
    Guid Id,
    string Name
);

public record GetAccountByIdPaymentTypeResponse(
    Guid Id,
    string Name
);
public static class GetAccountByIdEndpoint
{
    public static void MapGetAccountByIdEndpoint(this WebApplication app)
    {
        app.MapGet("/accounts/{AccountId}", async (
            Guid AccountId,
            IMediator mediator,
            HttpContext httpContext) =>
        {
            var currentUser = httpContext.GetCurrentUser();
            var query = new GetAccountByIdQuery(AccountId, currentUser.Id);
            var result = await mediator.Send(query);

            return result.MatchResponse();
        });
    }
}

public record GetAccountByIdQuery(
    Guid AccountId,
    string UserId) : IRequest<ErrorOr<GetAccountByIdResponse>>;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, ErrorOr<GetAccountByIdResponse>>
{
    private readonly BudgetDbContext _dbContext;

    public GetAccountByIdQueryHandler(BudgetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<GetAccountByIdResponse>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Accounts
            .Where(a => a.UserId == request.UserId)
            .Where(a => a.Id == request.AccountId)
            .Select(a => new GetAccountByIdResponse(
                a.Id,
                a.Name,
                a.InitialBalance,
                a.InitialBalance + a.Records.Sum(r => r.Amount),
                new GetAccountByIdCurrencyResponse(
                    a.Currency.Id,
                    a.Currency.Name),
                new GetAccountByIdPaymentTypeResponse(
                    a.PaymentType.Id,
                    a.PaymentType.Name),
                a.IsActive))
            .FirstOrDefaultAsync();

        if (account is null)
        {
            return Errors.Account.NotFound;
        }

        return account;
    }
}
