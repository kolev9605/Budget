using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Records;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Budget.Application.Records.Commands;

public record UpdateRecordCommand(
    Guid Id,
    string Note,
    decimal Amount,
    Guid AccountId,
    Guid CategoryId,
    Guid PaymentTypeId,
    RecordType RecordType,
    DateTimeOffset RecordDate,
    Guid? FromAccountId,
    string UserId) : IRequest<ErrorOr<RecordModel>>;

public class UpdateRecordCommandHandler : IRequestHandler<UpdateRecordCommand, ErrorOr<RecordModel>>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IRecordRepository _recordRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IPaymentTypeRepository _paymentTypeRepository;

    public UpdateRecordCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IRecordRepository recordRepository,
        IAccountRepository accountRepository,
        UserManager<ApplicationUser> userManager,
        ICategoryRepository categoryRepository,
        IPaymentTypeRepository paymentTypeRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _recordRepository = recordRepository;
        _accountRepository = accountRepository;
        _userManager = userManager;
        _categoryRepository = categoryRepository;
        _paymentTypeRepository = paymentTypeRepository;
    }

    public async Task<ErrorOr<RecordModel>> Handle(UpdateRecordCommand command, CancellationToken cancellationToken)
    {
        var record = await _recordRepository.GetRecordByIdAsync(command.Id, command.UserId);

        if (record == null)
        {
            return Errors.Record.NotFound;
        }

        // TODO: Validation logic. Move to a behavior?
        var account = await _accountRepository.GetForRecordCreationAsync(command.AccountId);
        if (account is null)
        {
            return Errors.Record.NotFound;
        }

        if (account.UserId != command.UserId)
        {
            return Errors.Account.BelongsToAnotherUser;
        }

        var user = await _userManager.FindByIdAsync(command.UserId);
        if (user is null)
        {
            return Errors.User.NotFound;
        }

        var category = await _categoryRepository.GetForRecordCreationAsync(command.CategoryId);
        if (category is null)
        {
            return Errors.Category.NotFound;
        }

        var paymentType = await _paymentTypeRepository.GetForRecordCreationAsync(command.PaymentTypeId);
        if (paymentType == null)
        {
            return Errors.PaymentType.NotFound;
        }

        record.Update(
            command.Note,
            command.RecordDate,
            command.Amount,
            command.AccountId,
            command.FromAccountId,
            command.PaymentTypeId,
            command.CategoryId,
            command.RecordType,
            _dateTimeProvider.UtcNow);

        if (command.RecordType == RecordType.Transfer)
        {
            // TODO: transfer validation logic. Behavior?
            if (!command.FromAccountId.HasValue)
            {
                return Errors.Account.NotFound;
            }

            var fromAccount = await _accountRepository.GetForRecordCreationAsync(command.FromAccountId.Value);
            if (fromAccount == null)
            {
                return Errors.Account.NotFound;
            }

            if (account.Id == fromAccount.Id)
            {
                return Errors.Record.SameAccountsInTransfer;
            }

            var existingTransferRecord = await _recordRepository.GetNegativeTransferRecordAsync(record);
            if (existingTransferRecord is null)
            {
                return Errors.Record.NotFound;
            }

            existingTransferRecord.Update(
                command.Note,
                command.RecordDate,
                command.Amount,
                accountId: command.FromAccountId.Value,
                fromAccountId: record.AccountId,
                command.PaymentTypeId,
                command.CategoryId,
                command.RecordType,
                _dateTimeProvider.UtcNow,
                true);

            var updatedExistingRecord = await _recordRepository.UpdateAsync(existingTransferRecord);

            record.FromAccountId = updatedExistingRecord.AccountId;
        }
        else
        {
            record.FromAccountId = null;
        }

        var updatedRecord = await _recordRepository.UpdateAsync(record);

        return updatedRecord.Adapt<RecordModel>();
    }
}
