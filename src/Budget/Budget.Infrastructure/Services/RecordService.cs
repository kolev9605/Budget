using Budget.Core.Constants;
using Budget.Core.Entities;
using Budget.Core.Exceptions;
using Budget.Core.Interfaces;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Records;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Infrastructure.Services
{
    public class RecordService : IRecordService
    {
        private readonly IRecordRepository _recordRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IRepository<Category> _categoriesRepository;
        private readonly IRepository<PaymentType> _paymentTypesRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecordService(
            IRecordRepository recordsRepository,
            IAccountRepository accountRepository,
            IDateTimeProvider dateTimeProvider,
            IRepository<Category> categoriesRepository,
            IRepository<PaymentType> paymentTypesRepository,
            UserManager<ApplicationUser> userManager)
        {
            _recordRepository = recordsRepository;
            _accountRepository = accountRepository;
            _dateTimeProvider = dateTimeProvider;
            _categoriesRepository = categoriesRepository;
            _paymentTypesRepository = paymentTypesRepository;
            _userManager = userManager;
        }

        public async Task<RecordModel> GetByIdAsync(int id, string userId)
        {
            var record = await _recordRepository.GetRecordByIdAsync(id, userId);

            return RecordModel.FromRecord(record);
        }

        /// <summary>
        /// Gets the record for update. In case of updating a transfer record, returning the positive among the two records.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<RecordModel> GetByIdForUpdateAsync(int id, string userId)
        {
            var record = await _recordRepository.GetRecordByIdAsync(id, userId);
            
            // Only the positive transfer record should be edited to simplify the update process
            if (record.RecordType == RecordType.Transfer)
            {
                var positiveTransferRecord = await _recordRepository.GetPositiveTransferRecordAsync(record);
                return RecordModel.FromRecord(positiveTransferRecord);
            }

            return RecordModel.FromRecord(record);
        }

        public async Task<IEnumerable<RecordsGroupModel>> GetAllAsync(string userId)
        {
            var records = await _recordRepository.GetAllAsync(userId);

            var models = new List<RecordsGroupModel>();

            var recordsGroupedByDate = records
                .GroupBy(r => r.RecordDate.Date)
                .ToDictionary(r => r.Key, r => r.ToList())
                .OrderByDescending(r => r.Key)
                .Select(r => new RecordsGroupModel()
                {
                    Date = r.Key,
                    Sum = r.Value.Sum(r => r.Amount),
                    Records = r.Value.Select(rm => RecordModel.FromRecord(rm))
                });

            return recordsGroupedByDate;
        }

        public async Task<int> CreateAsync(CreateRecordModel createRecordModel, string userId)
        {
            await ValidateCrudRecordModel(createRecordModel, userId);

            var now = _dateTimeProvider.Now;
            var record = new Record()
            {
                AccountId = createRecordModel.AccountId,
                Amount = GetAmountByRecordType(createRecordModel.Amount, createRecordModel.RecordType),
                Note = createRecordModel.Note,
                DateCreated = now,
                CategoryId = createRecordModel.CategoryId,
                PaymentTypeId = createRecordModel.PaymentTypeId,
                RecordType = createRecordModel.RecordType,
                RecordDate = createRecordModel.RecordDate,
            };

            if (createRecordModel.FromAccountId.HasValue && createRecordModel.RecordType == RecordType.Transfer)
            {
                await ValidateTransferRecord(createRecordModel.AccountId, createRecordModel.FromAccountId.Value);

                var negativeTransferRecord = await CreateNegativeTransferRecord(createRecordModel, now);

                record.FromAccountId = negativeTransferRecord.AccountId;
            }

            var createdRecord = await _recordRepository.CreateAsync(record);

            return createdRecord.Id;
        }

        public async Task<int> UpdateAsync(UpdateRecordModel updateRecordModel, string userId)
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

            if (updateRecordModel.FromAccountId.HasValue && updateRecordModel.RecordType == RecordType.Transfer)
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

                await _recordRepository.UpdateAsync(existingTransferRecord);

                record.FromAccountId = existingTransferRecord.AccountId;
            }
            else
            {
                record.FromAccountId = null;
            }

            var updatedRecord = await _recordRepository.UpdateAsync(record);

            return updatedRecord.Id;
        }

        public async Task<int> DeleteAsync(int recordId, string userId)
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
                await _recordRepository.DeleteAsync(existingTransferRecord.Id);
            }

            var deletedRecord = await _recordRepository.DeleteAsync(recordId);

            return deletedRecord.Id;
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

        private async Task ValidateTransferRecord(int accountId, int fromAccountId)
        {
            var account = await _accountRepository.BaseGetByIdAsync(accountId);
            if (account == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(account)));
            }

            var fromAccount = await _accountRepository.BaseGetByIdAsync(fromAccountId);
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
}
