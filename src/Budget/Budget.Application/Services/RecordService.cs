using Budget.Domain.Constants;
using Budget.Domain.Entities;
using Budget.Domain.Exceptions;
using Budget.Domain.Interfaces;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Pagination;
using Budget.Domain.Models.Records;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Application.Services;

public class RecordService : IRecordService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IRecordRepository _recordRepository;
    private readonly ICategoryRepository _categoriesRepository;
    private readonly IRepository<PaymentType> _paymentTypesRepository;
    private readonly IAccountRepository _accountRepository;

    public RecordService(
        UserManager<ApplicationUser> userManager,
        IDateTimeProvider dateTimeProvider,
        IRecordRepository recordRepository,
        ICategoryRepository categoriesRepository,
        IRepository<PaymentType> paymentTypesRepository,
        IAccountRepository accountRepository)
    {
        _userManager = userManager;
        _dateTimeProvider = dateTimeProvider;
        _recordRepository = recordRepository;
        _categoriesRepository = categoriesRepository;
        _paymentTypesRepository = paymentTypesRepository;
        _accountRepository = accountRepository;
    }

    public async Task<RecordModel> GetByIdAsync(int recordId, string userId)
    {
        var record = await _recordRepository.GetRecordByIdMappedAsync(recordId, userId);

        return record;
    }

    /// <summary>
    /// Gets the record for update. In case of updating a transfer record, returning the positive among the two records.
    /// </summary>
    /// <param name="recordId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<RecordModel> GetByIdForUpdateAsync(int recordId, string userId)
    {
        var record = await _recordRepository.GetRecordByIdMappedAsync(recordId, userId);

        // Only the positive transfer record should be edited to simplify the update process
        if (record.RecordType == RecordType.Transfer)
        {
            var positiveTransferRecord = await _recordRepository.GetPositiveTransferRecordMappedAsync(record.RecordDate, record.Category.Id, record.Amount);

            return positiveTransferRecord;
        }

        return record;
    }

    public async Task<IEnumerable<RecordsExportModel>> GetAllForExportAsync(string userId)
    {
        var records = await _recordRepository.GetAllForExportAsync(userId);

        return records;
    }

    public async Task<IPagedListContainer<RecordModel>> GetAllPaginatedAsync(PaginatedRequestModel requestModel, string userId)
    {
        var paginated = await _recordRepository.GetAllPaginatedAsync(userId, requestModel);

        return paginated;
    }

    public async Task<RecordModel> CreateAsync(CreateRecordModel createRecordModel, string userId)
    {
        await ValidateCrudRecordModel(createRecordModel, userId);

        var now = _dateTimeProvider.UtcNow;
        var amount = GetAmountByRecordType(createRecordModel.Amount, createRecordModel.RecordType);

        var record = (createRecordModel, now, amount).Adapt<Record>();

        if (createRecordModel.RecordType == RecordType.Transfer)
        {
            await ValidateTransferRecord(createRecordModel.AccountId, createRecordModel.FromAccountId);

            var negativeTransferRecord = await CreateNegativeTransferRecord(createRecordModel, now);

            record.FromAccountId = negativeTransferRecord.AccountId;
        }

        var createdRecord = await _recordRepository.CreateAsync(record);

        return createdRecord.Adapt<RecordModel>();
    }

    public async Task<RecordModel> UpdateAsync(UpdateRecordModel updateRecordModel, string userId)
    {
        var record = await _recordRepository.GetRecordByIdAsync(updateRecordModel.Id, userId);

        if (record == null)
        {
            throw new BudgetValidationException(
                string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(record)));
        }

        await ValidateCrudRecordModel(updateRecordModel, userId);
        var existingTransferRecord = await _recordRepository.GetNegativeTransferRecordAsync(record);

        record.AccountId = updateRecordModel.AccountId;
        record.Amount = GetAmountByRecordType(updateRecordModel.Amount, updateRecordModel.RecordType);
        record.Note = updateRecordModel.Note;
        record.CategoryId = updateRecordModel.CategoryId;
        record.PaymentTypeId = updateRecordModel.PaymentTypeId;
        record.RecordType = updateRecordModel.RecordType;
        record.RecordDate = updateRecordModel.RecordDate;

        if (updateRecordModel.RecordType == RecordType.Transfer)
        {
            await ValidateTransferRecord(updateRecordModel.AccountId, updateRecordModel.FromAccountId.Value);

            existingTransferRecord.AccountId = updateRecordModel.FromAccountId.Value;
            existingTransferRecord.Amount = GetAmountByRecordType(record.Amount, existingTransferRecord.RecordType, true);
            existingTransferRecord.Note = updateRecordModel.Note;
            existingTransferRecord.CategoryId = updateRecordModel.CategoryId;
            existingTransferRecord.PaymentTypeId = updateRecordModel.PaymentTypeId;
            existingTransferRecord.RecordType = updateRecordModel.RecordType;
            existingTransferRecord.RecordDate = updateRecordModel.RecordDate;
            existingTransferRecord.FromAccountId = record.AccountId;

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

    public async Task<RecordModel> DeleteAsync(int recordId, string userId)
    {
        var record = await _recordRepository.GetRecordByIdAsync(recordId, userId);

        if (record == null)
        {
            throw new BudgetValidationException(
                string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(record)));
        }

        var existingTransferRecord = await _recordRepository.GetNegativeTransferRecordAsync(record);

        if (existingTransferRecord != null)
        {
            await _recordRepository.DeleteAsync(existingTransferRecord);
        }

        var deletedRecord = await _recordRepository.DeleteAsync(record);

        return deletedRecord.Adapt<RecordModel>();
    }

    public async Task<RecordsDateRangeModel> GetRecordsDateRangeAsync(string userId)
    {
        var allRecords = await _recordRepository.GetAllAsync(userId);

        if (!allRecords.Any())
        {
            return null;
        }

        //TODO: Mapster
        var minRecordDate = allRecords.Min(r => r.RecordDate);
        var maxRecordDate = allRecords.Max(r => r.RecordDate);

        return new RecordsDateRangeModel(minRecordDate, maxRecordDate);
    }

    private decimal GetAmountByRecordType(decimal amount, RecordType recordType, bool isNegativeTransferRecord = false)
    {
        if (recordType == RecordType.Expense || (recordType == RecordType.Transfer && isNegativeTransferRecord))
        {
            return -Math.Abs(amount);
        }

        return Math.Abs(amount);
    }

    private async Task<Record> CreateNegativeTransferRecord(BaseCrudRecordModel createRecordModel, DateTime date)
    {
        var negativeTransferRecord = new Record()
        {
            AccountId = createRecordModel.FromAccountId.Value,
            Amount = GetAmountByRecordType(createRecordModel.Amount, createRecordModel.RecordType, true),
            DateCreated = date,
            Note = createRecordModel.Note,
            CategoryId = createRecordModel.CategoryId,
            PaymentTypeId = createRecordModel.PaymentTypeId,
            RecordType = createRecordModel.RecordType,
            RecordDate = createRecordModel.RecordDate,
            FromAccountId = createRecordModel.AccountId,
        };

        var negativeTransferRecordCreated = await _recordRepository.CreateAsync(negativeTransferRecord);

        return negativeTransferRecordCreated;
    }

    private async Task ValidateCrudRecordModel(BaseCrudRecordModel model, string userId)
    {
        if (model == null)
        {
            throw new BudgetValidationException(
                string.Format(ValidationMessages.Common.IsNotNull, nameof(model)));
        }

        await ValidateAccount(model.AccountId, userId);

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new BudgetValidationException(
                string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(user)));
        }

        var category = await _categoriesRepository.BaseGetByIdAsync(model.CategoryId);
        if (category == null)
        {
            throw new BudgetValidationException(
                string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(category)));
        }

        var paymentType = await _paymentTypesRepository.BaseGetByIdAsync(model.PaymentTypeId);
        if (paymentType == null)
        {
            throw new BudgetValidationException(
                string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(paymentType)));
        }
    }

    private async Task ValidateAccount(int accountId, string userId)
    {
        var account = await _accountRepository.BaseGetByIdAsync(accountId);
        if (account == null)
        {
            throw new BudgetValidationException(
                string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(account)));
        }

        if (account.UserId != userId)
        {
            throw new BudgetValidationException(
                string.Format(ValidationMessages.Accounts.InvalidAccount, account.Name));
        }
    }

    private async Task ValidateTransferRecord(int accountId, int? fromAccountId)
    {
        if (!fromAccountId.HasValue)
        {
            throw new BudgetValidationException(
                string.Format(ValidationMessages.Common.IsNotNull, nameof(fromAccountId)));
        }

        var account = await _accountRepository.BaseGetByIdAsync(accountId);
        if (account == null)
        {
            throw new BudgetValidationException(
                string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(account)));
        }

        var fromAccount = await _accountRepository.BaseGetByIdAsync(fromAccountId.Value);
        if (fromAccount == null)
        {
            throw new BudgetValidationException(
                string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(fromAccount)));
        }

        if (account.Id == fromAccount.Id)
        {
            throw new BudgetValidationException(ValidationMessages.Accounts.SameAccountsInTransfer);
        }
    }
}
