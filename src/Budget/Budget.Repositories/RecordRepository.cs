using Budget.Core.Entities;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Pagination;
using Budget.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Repositories
{
    public class RecordRepository : Repository<Record>, IRecordRepository
    {
        private readonly IPaginationManager _paginationManager;

        public RecordRepository(
            BudgetDbContext budgetDbContext,
            IPaginationManager paginationManager) : base(budgetDbContext)
        {
            _paginationManager = paginationManager;
        }

        public async Task<Record> GetRecordByIdAsync(int id, string userId)
        {
            var record = await _budgetDbContext.Records
                .Include(r => r.Account)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.FromAccount)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.PaymentType)
                .Include(r => r.Category)
                .Where(r => r.Account.UserId == userId)
                .FirstOrDefaultAsync(r => r.Id == id);

            return record;
        }

        public async Task<Record> GetPositiveTransferRecordAsync(Record record)
        {
            var fromAccountRecord = await _budgetDbContext.Records
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
                .Where(r => r.CategoryId == record.CategoryId)
                .FirstOrDefaultAsync();

            return fromAccountRecord;
        }

        public async Task<Record> GetNegativeTransferRecordAsync(Record record)
        {
            var transferRecord = await _budgetDbContext.Records
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

            return transferRecord;
        }

        public async Task<IEnumerable<Record>> GetAllAsync(string userId)
        {
            var records = await _budgetDbContext.Records
                .Include(r => r.Account)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.FromAccount)
                .Include(r => r.PaymentType)
                .Include(r => r.Category)
                .Where(r => r.Account.UserId == userId)
                .OrderByDescending(r => r.RecordDate)
                .ToListAsync();

            return records;
        }

        public async Task<PaginationModel<Record>> GetAllPaginatedAsync(string userId, PaginatedRequestModel queryStringParameters)
        {
            var query = _budgetDbContext.Records
                .Include(r => r.Account)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.FromAccount)
                .Include(r => r.PaymentType)
                .Include(r => r.Category)
                .Where(r => r.Account.UserId == userId)
                .OrderByDescending(r => r.RecordDate);

            var paginatedRecords = await _paginationManager.CreateAsync(query, queryStringParameters.PageNumber, queryStringParameters.PageSize);

            return paginatedRecords;
        }

        public async Task<IEnumerable<Record>> GetAllInRangeAndAccountsAsync(string userId, DateTime startDate, DateTime endDate, IEnumerable<int> accountIds)
        {
            var records = await _budgetDbContext.Records
                .Include(r => r.Account)
                .Where(r => r.Account.UserId == userId)
                .Where(r => r.RecordDate >= startDate && r.RecordDate <= endDate)
                .Where(r => accountIds.Contains(r.AccountId))
                .OrderBy(r => r.RecordDate)
                .ToListAsync();

            return records;
        }

        public async Task<IEnumerable<Record>> GetAllInRangeAsync(string userId, DateTime startDate, DateTime endDate)
        {
            var records = await _budgetDbContext.Records
                .Include(r => r.Account)
                .Where(r => r.Account.UserId == userId)
                .Where(r => r.RecordDate >= startDate && r.RecordDate <= endDate)
                .OrderBy(r => r.RecordDate)
                .ToListAsync();

            return records;
        }
    }
}
