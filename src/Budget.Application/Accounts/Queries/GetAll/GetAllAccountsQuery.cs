using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Accounts;
using ErrorOr;
using MediatR;

namespace Budget.Application.Accounts.Queries.GetAll;

public record GetAllAccountsQuery(
    string UserId,
    bool IncludeHidden) : IRequest<ErrorOr<IEnumerable<AccountModel>>>;

public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, ErrorOr<IEnumerable<AccountModel>>>
{
    private readonly IAccountRepository _accountRepository;

    public GetAllAccountsQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<ErrorOr<IEnumerable<AccountModel>>> Handle(GetAllAccountsQuery query, CancellationToken cancellationToken)
    {
        var accounts = await _accountRepository.GetAllAccountModelsByUserIdAsync(query.UserId, query.IncludeHidden);

        return accounts.ToErrorOr();
    }
}
