using Budget.Domain.Interfaces.Repositories;
using MediatR;

public record GetTotalBalanceQuery(
    string UserId) : IRequest<decimal>;

public class GetTotalBalanceQueryHandler : IRequestHandler<GetTotalBalanceQuery, decimal>
{
    private readonly IAccountRepository _accountRepository;

    public GetTotalBalanceQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<decimal> Handle(GetTotalBalanceQuery query, CancellationToken cancellationToken)
    {
        var balance = await _accountRepository.GetTotalBalanceByUserIdAsync(query.UserId);

        return balance;
    }
}
