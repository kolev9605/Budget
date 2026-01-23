using Budget.Domain.Common.Errors;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Accounts;
using ErrorOr;
using Mapster;
using MediatR;

namespace Budget.Application.Accounts.Commands;

public record UpdateAccountCommand(
    Guid Id,
    string Name,
    Guid CurrencyId,
    Guid PaymentTypeId,
    decimal InitialBalance,
    string UserId
) : IRequest<ErrorOr<AccountModel>>;

public class UpdateAccountCommandHandler(
    ICurrencyRepository _currencyRepository,
    IAccountRepository _accountRepository,
    IPaymentTypeRepository _paymentTypeRepository)
    : IRequestHandler<UpdateAccountCommand, ErrorOr<AccountModel>>
{
    public async Task<ErrorOr<AccountModel>> Handle(UpdateAccountCommand command, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdWithCurrencyAsync(command.Id, command.UserId);
        if (account is null)
        {
            return Errors.Account.NotFound;
        }

        if (account.UserId != command.UserId)
        {
            return Errors.Account.BelongsToAnotherUser;
        }

        var currency = await _currencyRepository.GetByIdAsync(command.CurrencyId);
        if (currency is null)
        {
            return Errors.Currency.NotFound;
        }

        var paymentType =  await _paymentTypeRepository.GetForRecordCreationAsync(command.PaymentTypeId);
        if (paymentType == null)
        {
            return Errors.PaymentType.NotFound;
        }

        account.CurrencyId = currency.Id;
        account.Name = command.Name;
        account.InitialBalance = command.InitialBalance;
        account.PaymentTypeId = paymentType.Id;

        var updatedAccount = await _accountRepository.UpdateAsync(account);

        return updatedAccount.Adapt<AccountModel>();
    }
}
