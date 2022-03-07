using Budget.Core.Constants;
using Budget.Core.Entities;
using Budget.Core.Exceptions;
using Budget.Core.Interfaces;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Records;
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

        public RecordService(
            IRecordRepository recordsRepository,
            IAccountRepository accountRepository, 
            IDateTimeProvider dateTimeProvider)
        {
            _recordRepository = recordsRepository;
            _accountRepository = accountRepository;
            _dateTimeProvider = dateTimeProvider;
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

        public async Task<int> CreateAsync(CreateRecordModel createRecordModel, string userId)
        {
            var account = await _accountRepository.GetByIdAsync(createRecordModel.AccountId);
            if (account == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(account), createRecordModel.AccountId));
            }

            if (account.UserId != userId)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Accounts.InvalidAccount, account.Name));
            }

            var record = new Record()
            {
                AccountId = account.Id,
                Amount = createRecordModel.Amount,
                DateAdded = _dateTimeProvider.Now,
                Note = createRecordModel.Note,
                CategoryId = createRecordModel.CategoryId,
                PaymentTypeId = createRecordModel.PaymentTypeId,
                RecordType = createRecordModel.RecordType,
            };

            var createdRecord = await _recordRepository.CreateAsync(record);

            return createdRecord.Id;
        }

        public async Task<int> UpdateAsync(UpdateRecordModel updateRecordModel)
        {
            var record = await _recordRepository.GetRecordByIdAsync(updateRecordModel.Id);
            if (record == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(record), updateRecordModel.Id));
            }

            record.AccountId = updateRecordModel.AccountId;
            record.Amount = updateRecordModel.Amount;
            record.Note = updateRecordModel.Note;
            record.CategoryId = updateRecordModel.CategoryId;
            record.PaymentTypeId = updateRecordModel.PaymentTypeId;
            record.RecordType = updateRecordModel.RecordType;

            var updatedRecord = await _recordRepository.UpdateAsync(record);

            return updatedRecord.Id;
        }

        public async Task<int> DeleteAsync(int recordId)
        {
            var record = await _recordRepository.GetByIdAsync(recordId);
            if (record == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(record), recordId));
            }

            var deletedRecord = await _recordRepository.DeleteAsync(recordId);

            return deletedRecord.Id;
        }
    }
}
