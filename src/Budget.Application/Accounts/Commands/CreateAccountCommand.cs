using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Accounts;
using ErrorOr;
using Mapster;
using MediatR;

namespace Budget.Application.Accounts.Commands;

public record CreateAccountCommand(
    string Name,
    Guid CurrencyId,
    decimal InitialBalance,
    Guid PaymentTypeId,
    string UserId
) : IRequest<ErrorOr<AccountModel>>;

public class CreateAccountCommandHandler(
    ICurrencyRepository _currencyRepository,
    IAccountRepository _accountRepository,
    IPaymentTypeRepository _paymentTypeRepository)
    : IRequestHandler<CreateAccountCommand, ErrorOr<AccountModel>>
{
    public async Task<ErrorOr<AccountModel>> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var currency = await _currencyRepository.GetByIdAsync(command.CurrencyId);
        if (currency == null)
        {
            return Errors.Currency.NotFound;
        }

        var paymentType =  await _paymentTypeRepository.GetForRecordCreationAsync(command.PaymentTypeId);
        if (paymentType == null)
        {
            return Errors.PaymentType.NotFound;
        }

        var account = command.Adapt<Account>();

        var createdAccount = await _accountRepository.CreateAsync(account);

        return createdAccount.Adapt<AccountModel>();
    }
}
