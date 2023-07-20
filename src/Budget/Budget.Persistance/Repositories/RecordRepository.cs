using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Pagination;
using Budget.Persistance.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Persistance.Repositories
{
    public class RecordRepository : Repository<Record>, IRecordRepository
    {
        public RecordRepository(
            BudgetDbContext budgetDbContext) : base(budgetDbContext)
        {
        }

        public async Task<TResult> GetRecordByIdAsync<TResult>(int id, string userId)
        {
            var record = await GetAll()
                .Include(r => r.Account)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.FromAccount)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.PaymentType)
                .Include(r => r.Category)
                .Where(r => r.Account.UserId == userId)
                .Where(r => r.Id == id)
                .ProjectToType<TResult>()
                .FirstOrDefaultAsync();

            return record;
        }

        public async Task<TResult> GetPositiveTransferRecordAsync<TResult>(DateTime recordDate, int categoryId, decimal recordAmount)
        {
            var fromAccountRecord = await GetAll()
                .Include(r => r.Account)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.FromAccount)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.PaymentType)
                .Include(r => r.Category)
                .Where(r => Math.Abs(r.Amount) == Math.Abs(recordAmount))
                .Where(r => r.Amount > 0)
                .Where(r => r.RecordType == RecordType.Transfer)
                .Where(r => r.RecordDate == recordDate)
                .Where(r => r.CategoryId == categoryId)
                .ProjectToType<TResult>()
                .FirstOrDefaultAsync();

            return fromAccountRecord;
        }

        public async Task<TResult> GetNegativeTransferRecordAsync<TResult>(Record record)
        {
            var transferRecord = await GetAll()
                .Include(r => r.Account)
                .Where(r => r.Account.UserId == r.Account.UserId)
                .Where(r => r.AccountId == record.FromAccountId.GetValueOrDefault())
                .Where(r => r.FromAccountId.GetValueOrDefault() == record.AccountId)
                .Where(r => Math.Abs(r.Amount) == Math.Abs(record.Amount))
                .Where(r => r.RecordType == RecordType.Transfer)
                .Where(r => r.DateCreated == record.DateCreated)
                .Where(r => r.CategoryId == record.CategoryId)
                .Where(r => r.Id != record.Id)
                .ProjectToType<TResult>()
                .FirstOrDefaultAsync();

            return transferRecord;
        }

        public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(string userId)
        {
            var records = await GetAll()
                .Include(r => r.Account)
                    .ThenInclude(a => a.Currency)
                .Include(r => r.FromAccount)
                .Include(r => r.PaymentType)
                .Include(r => r.Category)
                .Where(r => r.Account.UserId == userId)
                .OrderByDescending(r => r.RecordDate)
                .ProjectToType<TResult>()
                .ToListAsync();

            return records;
        }

        //public async Task<IDictionary<Account, IEnumerable<Record>>> GetRecordsGroupedByAccount(string userId)
        //{
        //    var records = await _budgetDbContext.Records
        //        .Include(r => r.Account)
        //            .ThenInclude(a => a.Currency)
        //        .Include(r => r.FromAccount)
        //        .Include(r => r.PaymentType)
        //        .Include(r => r.Category)
        //        .Where(r => r.Account.UserId == userId)
        //        .GroupBy(r => r.Account)
        //        .ToDictionaryAsync(r => r.Key, r => r.AsEnumerable());

        //    return records;
        //}

        public async Task<IPagedListContainer<TResult>> GetAllPaginatedAsync<TResult>(string userId, PaginatedRequestModel queryStringParameters)
        {
           var paginatedRecords = await _budgetDbContext.Records
               .Include(r => r.Account)
                   .ThenInclude(a => a.Currency)
               .Include(r => r.FromAccount)
               .Include(r => r.PaymentType)
               .Include(r => r.Category)
               .Where(r => r.Account.UserId == userId)
               .OrderByDescending(r => r.RecordDate)
               .ProjectToType<TResult>()
               .PaginateAsync(queryStringParameters.PageNumber, queryStringParameters.PageSize);

           return paginatedRecords;
        }

        public async Task<IEnumerable<TResult>> GetAllInRangeAndAccountsAsync<TResult>(string userId, DateTime startDate, DateTime endDate, IEnumerable<int> accountIds)
        {
            var records = await GetAll()
                .Include(r => r.Account)
                .Where(r => r.Account.UserId == userId)
                .Where(r => r.RecordDate >= startDate && r.RecordDate <= endDate)
                .Where(r => accountIds.Contains(r.AccountId))
                .OrderBy(r => r.RecordDate)
                .ProjectToType<TResult>()
                .ToListAsync();

            return records;
        }

        public async Task<IEnumerable<TResult>> GetAllInRangeAsync<TResult>(string userId, DateTime startDate, DateTime endDate)
        {
            var records = await GetAll()
                .Include(r => r.Account)
                .Where(r => r.Account.UserId == userId)
                .Where(r => r.RecordDate >= startDate && r.RecordDate <= endDate)
                .OrderBy(r => r.RecordDate)
                .ProjectToType<TResult>()
                .ToListAsync();

            return records;
        }
    }
}
