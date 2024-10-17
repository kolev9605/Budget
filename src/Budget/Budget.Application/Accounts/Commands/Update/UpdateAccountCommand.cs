using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Accounts;
using ErrorOr;
using Mapster;
using MediatR;

namespace Budget.Application.Accounts.Commands.Create;

public record UpdateAccountCommand(
    Guid Id,
    string Name,
    Guid CurrencyId,
    decimal InitialBalance,
    string UserId
) : IRequest<ErrorOr<AccountModel>>;

public class UpdateAccountCommandHandler(
    IRepository<Currency> _currencyRepository,
    IAccountRepository _accountRepository)
    : IRequestHandler<UpdateAccountCommand, ErrorOr<AccountModel>>
{
    public async Task<ErrorOr<AccountModel>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdWithCurrencyAsync(request.Id, request.UserId);
        if (account is null)
        {
            return Errors.Account.AccountNotFound;
        }

        if (account.UserId != request.UserId)
        {
            return Errors.Account.AccountBelongsToAnotherUser;
        }

        var currency = await _currencyRepository.BaseGetByIdAsync(request.CurrencyId);
        if (currency is null)
        {
            return Errors.Currency.CurrencyNotFound;
        }

        account.CurrencyId = currency.Id;
        account.Name = request.Name;
        account.InitialBalance = request.InitialBalance;

        var updatedAccount = await _accountRepository.UpdateAsync(account);

        return updatedAccount.Adapt<AccountModel>();
    }
}
