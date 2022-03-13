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

        public async Task<RecordModel> GetByIdAsync(int id)
        {
            var recordEntity = await _recordRepository.GetRecordByIdAsync(id);

            var recordDto = RecordModel.FromRecord(recordEntity);

            return recordDto;
        }

        public async Task<IEnumerable<RecordModel>> GetAllAsync()
        {
            var records = await _recordRepository.GetAllAsync();

            var recordModels = records
                .ToList()
                .Select(r => RecordModel.FromRecord(r));

            return recordModels;
        }

        public async Task<RecordModel> CreateAsync(CreateRecordModel createRecordModel, string userId)
        {
            await ValidateCrudRecordModel(createRecordModel, userId);

            var record = new Record()
            {
                AccountId = createRecordModel.AccountId,
                Amount = GetAmountByRecordType(createRecordModel.Amount, createRecordModel.RecordType),
                DateAdded = _dateTimeProvider.Now,
                Note = createRecordModel.Note,
                CategoryId = createRecordModel.CategoryId,
                PaymentTypeId = createRecordModel.PaymentTypeId,
                RecordType = createRecordModel.RecordType,
            };

            var createdRecord = await _recordRepository.CreateAsync(record);

            return RecordModel.FromRecord(createdRecord);
        }

        public async Task<RecordModel> UpdateAsync(UpdateRecordModel updateRecordModel, string userId)
        {
            var record = await _recordRepository.GetRecordByIdAsync(updateRecordModel.Id);
            if (record == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(record), updateRecordModel.Id));
            }

            await ValidateCrudRecordModel(updateRecordModel, userId);

            record.AccountId = updateRecordModel.AccountId;
            record.Amount = GetAmountByRecordType(updateRecordModel.Amount, updateRecordModel.RecordType);
            record.Note = updateRecordModel.Note;
            record.CategoryId = updateRecordModel.CategoryId;
            record.PaymentTypeId = updateRecordModel.PaymentTypeId;
            record.RecordType = updateRecordModel.RecordType;

            var updatedRecord = await _recordRepository.UpdateAsync(record);

            return RecordModel.FromRecord(updatedRecord);
        }

        public async Task<RecordModel> DeleteAsync(int recordId)
        {
            var record = await _recordRepository.GetByIdAsync(recordId);
            if (record == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(record), recordId));
            }

            var deletedRecord = await _recordRepository.DeleteAsync(recordId);

            return RecordModel.FromRecord(deletedRecord);
        }

        private decimal GetAmountByRecordType(decimal amount, RecordType recordType)
        {
            if (recordType == RecordType.Expense)
            {
                return -Math.Abs(amount);
            }

            return Math.Abs(amount);
        }

        private async Task ValidateCrudRecordModel(BaseCrudRecordModel model, string userId)
        {
            if (model == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.IsNotNull, nameof(model)));
            }

            var account = await _accountRepository.GetByIdAsync(model.AccountId);
            if (account == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(account), model.AccountId));
            }

            if (account.UserId != userId)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Accounts.InvalidAccount, account.Name));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(user), userId));
            }

            var category = await _categoriesRepository.GetByIdAsync(model.CategoryId);
            if (category == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(category), model.CategoryId));
            }

            var paymentType = await _paymentTypesRepository.GetByIdAsync(model.PaymentTypeId);
            if (paymentType == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(paymentType), model.PaymentTypeId));
            }
        }
    }
}
