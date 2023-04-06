using Budget.Application.Extensions;
using Budget.Application.Interfaces;
using Budget.Application.Interfaces.Services;
using Budget.Application.Models.Pagination;
using Budget.Application.Models.Records;
using Budget.Domain.Constants;
using Budget.Domain.Entities;
using Budget.Domain.Exceptions;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Application.Services
{
    public class RecordService : IRecordService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBudgetDbContext _budgetDbContext;
        private readonly IPaginationManager _paginationManager;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RecordService(
            UserManager<ApplicationUser> userManager,
            IBudgetDbContext budgetDbContext,
            IPaginationManager paginationManager,
            IDateTimeProvider dateTimeProvider)
        {
            _userManager = userManager;
            _budgetDbContext = budgetDbContext;
            _paginationManager = paginationManager;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<RecordModel> GetByIdAsync(int id, string userId)
        {
            var record = await _budgetDbContext.Records
                .Include(r => r.Account)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.FromAccount)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.PaymentType)
                .Include(r => r.Category)
                .Where(r => r.Account.UserId == userId)
                .ProjectToType<RecordModel>()
                .FirstOrDefaultAsync(r => r.Id == id);

            return record;
        }

        /// <summary>
        /// Gets the record for update. In case of updating a transfer record, returning the positive among the two records.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<RecordModel> GetByIdForUpdateAsync(int id, string userId)
        {
            var record = await _budgetDbContext.Records
                .Include(r => r.Account)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.FromAccount)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.PaymentType)
                .Include(r => r.Category)
                .Where(r => r.Account.UserId == userId)
                .ProjectToType<RecordModel>()
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            // Only the positive transfer record should be edited to simplify the update process
            if (record.RecordType == RecordType.Transfer)
            {
                var positiveTransferRecord = await _budgetDbContext.Records
                    .Include(r => r.Account)
                        .ThenInclude(a => a.Currency)
                    .Include(r => r.FromAccount)
                        .ThenInclude(a => a.Currency)
                    .Include(r => r.PaymentType)
                    .Include(r => r.Category)
                    .Where(r => Math.Abs(r.Amount) == Math.Abs(record.Amount))
                    .Where(r => r.Amount > 0)
                    .Where(r => r.RecordType == RecordType.Transfer)
                    .Where(r => r.RecordDate == record.RecordDate)
                    .Where(r => r.CategoryId == record.Category.Id)
                    .ProjectToType<RecordModel>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                return positiveTransferRecord;
            }

            return record;
        }

        public async Task<IEnumerable<RecordsExportModel>> GetAllForExportAsync(string userId)
        {
            var records = await _budgetDbContext.Records
                .Include(r => r.Account)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.FromAccount)
                .Include(r => r.PaymentType)
                .Include(r => r.Category)
                .Where(r => r.Account.UserId == userId)
                .OrderByDescending(r => r.RecordDate)
                .ProjectToType<RecordsExportModel>()
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<PaginationModel<RecordsGroupModel>> GetAllPaginatedAsync(PaginatedRequestModel requestModel, string userId)
        {
            var recordsQuery = _budgetDbContext.Records
                .Include(r => r.Account)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.FromAccount)
                .Include(r => r.PaymentType)
                .Include(r => r.Category)
                .Where(r => r.Account.UserId == userId)
                .OrderByDescending(r => r.RecordDate);

            var paginatedRecords = await _paginationManager.CreateAsync(recordsQuery, requestModel.PageNumber, requestModel.PageSize);

            // The RecordDate is converted to local time before the grouping to produce groups based on Local time
            paginatedRecords.Items.ForEach(r => r.RecordDate = r.RecordDate.ToLocalTime());

            var recordsGroupedByDate = paginatedRecords.Items
                .GroupBy(r => r.RecordDate.Date)
                .ToDictionary(r => r.Key, r => r.ToList())
                .OrderByDescending(r => r.Key)
                .Select(r => new RecordsGroupModel()
                {
                    Date = r.Key,
                    Sum = r.Value.Sum(r => r.Amount),
                    Records = r.Value.Select(rm => rm.Adapt<RecordModel>())
                });

            var paginatedRecordModels = paginatedRecords.Convert(recordsGroupedByDate.ToList());

            return paginatedRecordModels;
        }

        public async Task<RecordModel> CreateAsync(CreateRecordModel createRecordModel, string userId)
        {
            await ValidateCrudRecordModel(createRecordModel, userId);

            var now = _dateTimeProvider.Now;
            var amount = GetAmountByRecordType(createRecordModel.Amount, createRecordModel.RecordType);

            var record = createRecordModel.Adapt<Record>();
            // TODO: Pass to mapster somehow
            record.DateCreated = now;

            if (createRecordModel.RecordType == RecordType.Transfer)
            {
                await ValidateTransferRecord(createRecordModel.AccountId, createRecordModel.FromAccountId);

                var negativeTransferRecord = await CreateNegativeTransferRecord(createRecordModel, now);

                record.FromAccountId = negativeTransferRecord.AccountId;
            }

            var createdRecord = await _budgetDbContext.Records.AddAsync(record);
            await _budgetDbContext.SaveChangesAsync();

            return createdRecord.Entity.Adapt<RecordModel>();
        }

        public async Task<RecordModel> UpdateAsync(UpdateRecordModel updateRecordModel, string userId)
        {
            var record = await _budgetDbContext.Records
                .Include(r => r.Account)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.FromAccount)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.PaymentType)
                .Include(r => r.Category)
                .Where(r => r.Account.UserId == userId)
                .FirstOrDefaultAsync(r => r.Id == updateRecordModel.Id);

            if (record == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(record)));
            }

            await ValidateCrudRecordModel(updateRecordModel, userId);

            var existingTransferRecord = await _budgetDbContext.Records
                .Include(r => r.Account)
                .Where(r => r.Account.UserId == r.Account.UserId)
                .Where(r => r.AccountId == record.FromAccountId.GetValueOrDefault())
                .Where(r => r.FromAccountId.GetValueOrDefault() == record.AccountId)
                .Where(r => Math.Abs(r.Amount) == Math.Abs(record.Amount))
                .Where(r => r.RecordType == RecordType.Transfer)
                .Where(r => r.DateCreated == record.DateCreated)
                .Where(r => r.CategoryId == record.CategoryId)
                .Where(r => r.Id != record.Id)
                .FirstOrDefaultAsync();

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

                _budgetDbContext.Records.Update(existingTransferRecord);
                await _budgetDbContext.SaveChangesAsync();

                record.FromAccountId = existingTransferRecord.AccountId;
            }
            else
            {
                record.FromAccountId = null;
            }

            var updatedRecord = _budgetDbContext.Records.Update(record); 
            await _budgetDbContext.SaveChangesAsync();

            return updatedRecord.Entity.Adapt<RecordModel>();
        }

        public async Task<RecordModel> DeleteAsync(int recordId, string userId)
        {
            var record = await _budgetDbContext.Records
                .Include(r => r.Account)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.FromAccount)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.PaymentType)
                .Include(r => r.Category)
                .Where(r => r.Account.UserId == userId)
                .FirstOrDefaultAsync(r => r.Id == recordId);

            if (record == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(record)));
            }

            var existingTransferRecord = await _budgetDbContext.Records
                .Include(r => r.Account)
                .Where(r => r.Account.UserId == r.Account.UserId)
                .Where(r => r.AccountId == record.FromAccountId.GetValueOrDefault())
                .Where(r => r.FromAccountId.GetValueOrDefault() == record.AccountId)
                .Where(r => Math.Abs(r.Amount) == Math.Abs(record.Amount))
                .Where(r => r.RecordType == RecordType.Transfer)
                .Where(r => r.DateCreated == record.DateCreated)
                .Where(r => r.CategoryId == record.CategoryId)
                .Where(r => r.Id != record.Id)
                .FirstOrDefaultAsync();

            if (existingTransferRecord != null)
            {
                _budgetDbContext.Records.Remove(existingTransferRecord); 
            }

            var deletedRecord = _budgetDbContext.Records.Remove(record);
            await _budgetDbContext.SaveChangesAsync();

            return deletedRecord.Entity.Adapt<RecordModel>();
        }

        public async Task<RecordsDateRangeModel> GetRecordsDateRangeAsync(string userId)
        {
            var allRecords = await _budgetDbContext.Records
                .Include(r => r.Account)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.FromAccount)
                .Include(r => r.PaymentType)
                .Include(r => r.Category)
                .Where(r => r.Account.UserId == userId)
                .OrderByDescending(r => r.RecordDate)
                .ToListAsync();

            if (!allRecords.Any())
            {
                return null;
            }

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

            var negativeTransferRecordCreated = await _budgetDbContext.Records.AddAsync(negativeTransferRecord);
            await _budgetDbContext.SaveChangesAsync();

            return negativeTransferRecordCreated.Entity;
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

            var category = await _budgetDbContext.Categories.FirstOrDefaultAsync(c => c.Id == model.CategoryId);
            if (category == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(category)));
            }

            var paymentType = await _budgetDbContext.PaymentTypes.FirstOrDefaultAsync(pt => pt.Id == model.PaymentTypeId);
            if (paymentType == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(paymentType)));
            }
        }

        private async Task ValidateAccount(int accountId, string userId)
        {
            var account = await _budgetDbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
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

            var account = await _budgetDbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
            if (account == null)
            {
                throw new BudgetValidationException(
                    string.Format(ValidationMessages.Common.EntityDoesNotExist, nameof(account)));
            }

            var fromAccount = await _budgetDbContext.Accounts.FirstOrDefaultAsync(a => a.Id == fromAccountId.Value);
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
