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

        public async Task<IEnumerable<RecordModel>> GetAllAsync()
        {
            var records = await _recordRepository.GetAllWithCurrenciesAsync();

            var recordDtos = records
                .ToList()
                .Select(r => RecordModel.FromRecord(r));

            return recordDtos;
        }

        public async Task<RecordModel> GetByIdAsync(int id)
        {
            var recordEntity = await _recordRepository.GetByIdWithCurrencyAsync(id);

            var recordDto = RecordModel.FromRecord(recordEntity);

            return recordDto;
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
    }
}
