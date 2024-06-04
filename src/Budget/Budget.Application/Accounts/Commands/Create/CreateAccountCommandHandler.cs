using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Accounts;
using ErrorOr;
using Mapster;
using MediatR;

namespace Budget.Application.Accounts.Commands.Create;

public record CreateAccountCommand(
    string Name,
    int CurrencyId,
    decimal InitialBalance,
    string UserId
) : IRequest<ErrorOr<AccountModel>>;

public class CreateAccountCommandHandler(
    IRepository<Currency> _currencyRepository,
    IAccountRepository _accountRepository)
    : IRequestHandler<CreateAccountCommand, ErrorOr<AccountModel>>
{
    public async Task<ErrorOr<AccountModel>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var currency = await _currencyRepository.BaseGetByIdAsync(request.CurrencyId);
        if (currency == null)
        {
            return Errors.Currency.CurrencyNotFound;
        }

        var account = request.Adapt<Account>();

        var createdAccount = await _accountRepository.CreateAsync(account);

        return createdAccount.Adapt<AccountModel>();
    }
}
