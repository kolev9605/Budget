using Budget.Core.Entities;
using Budget.Core.Interfaces.Repositories;
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
        public RecordRepository(BudgetDbContext budgetDbContext) : base(budgetDbContext)
        {
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
                .Where(r => r.DateCreated == record.DateCreated)
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

        public async Task<IEnumerable<Record>> GetAllByMonthAsync(string userId, int month)
        {
            var records = await _budgetDbContext.Records
                .Include(r => r.Account)
                .Where(r => r.Account.UserId == userId)
                .Where(r => r.RecordDate.Month == month)
                .OrderBy(r => r.RecordDate)
                .ToListAsync();

            return records;
        }
    }
}
