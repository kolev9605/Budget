using Budget.Domain.Common.Errors;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Accounts;
using ErrorOr;
using MediatR;

namespace Budget.Application.Accounts.Queries.GetById;

public record GetAccountByIdQuery(
    int AccountId,
    string UserId) : IRequest<ErrorOr<AccountModel>>;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, ErrorOr<AccountModel>>
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountByIdQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<ErrorOr<AccountModel>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetAccountModelByIdWithCurrencyAsync(request.AccountId, request.UserId);

        if (account is null)
        {
            return Errors.Account.AccountNotFound;
        }

        return account;
    }
}
