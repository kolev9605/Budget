using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Accounts;
using ErrorOr;
using Mapster;
using MediatR;

namespace Budget.Application.Accounts.Commands.Create;

public record DeleteAccountCommand(
    int AccountId,
    string UserId
) : IRequest<ErrorOr<AccountModel>>;

public class DeleteAccountCommandHandler(
    IAccountRepository _accountRepository)
    : IRequestHandler<DeleteAccountCommand, ErrorOr<AccountModel>>
{
    public async Task<ErrorOr<AccountModel>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdWithCurrencyAsync(request.AccountId, request.UserId);
        if (account == null)
        {
            return Errors.Account.AccountNotFound;
        }

        if (account.Records.Any())
        {
            return Errors.Account.AccountHasRecords;
        }

        await _accountRepository.DeleteAsync(account);

        return account.Adapt<AccountModel>();
    }
}
