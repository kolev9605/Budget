using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Pagination;
using Budget.Domain.Models.Records;
using Budget.Persistance.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Budget.Persistance.Repositories;

public class RecordRepository : Repository<Record>, IRecordRepository
{
    public RecordRepository(
        BudgetDbContext budgetDbContext) : base(budgetDbContext)
    {
    }

    public async Task<Record> GetRecordByIdAsync(Guid recordId, string userId)
    {
        var record = await GetRecordByIdBaseQuery(userId, recordId)
            .FirstOrDefaultAsync();

        return record;
    }

    public async Task<RecordModel> GetRecordByIdMappedAsync(Guid recordId, string userId)
    {
        var record = await GetRecordByIdBaseQuery(userId, recordId)
            .ProjectToType<RecordModel>()
            .FirstOrDefaultAsync();

        return record;
    }

    public async Task<RecordModel> GetPositiveTransferRecordMappedAsync(DateTime recordDate, Guid categoryId, decimal recordAmount)
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
            .ProjectToType<RecordModel>()
            .FirstOrDefaultAsync();

        return fromAccountRecord;
    }

    public async Task<Record> GetNegativeTransferRecordAsync(Record record)
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
            .FirstOrDefaultAsync();

        return transferRecord;
    }

    public async Task<IEnumerable<Record>> GetAllAsync(string userId)
    {
        var records = await GetAllBaseQuery(userId)
            .ToListAsync();

        return records;
    }

    public async Task<IEnumerable<RecordsExportModel>> GetAllForExportAsync(string userId)
    {
        var records = await GetAllBaseQuery(userId)
            .ProjectToType<RecordsExportModel>()
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

    public async Task<IPagedListContainer<RecordModel>> GetAllPaginatedAsync(string userId, PaginatedRequestModel queryStringParameters)
    {
       var paginatedRecords = await _budgetDbContext.Records
           .Include(r => r.Account)
               .ThenInclude(a => a.Currency)
           .Include(r => r.FromAccount)
           .Include(r => r.PaymentType)
           .Include(r => r.Category)
           .Where(r => r.Account.UserId == userId)
           .OrderByDescending(r => r.RecordDate)
           .ProjectToType<RecordModel>()
           .PaginateAsync(queryStringParameters.PageNumber, queryStringParameters.PageSize);

       return paginatedRecords;
    }

    public async Task<IEnumerable<Record>> GetAllInRangeAndAccountsAsync(string userId, DateTime startDate, DateTime endDate, IEnumerable<Guid> accountIds)
    {
        var records = await GetAll()
            .Include(r => r.Account)
            .Where(r => r.Account.UserId == userId)
            .Where(r => r.RecordDate >= startDate && r.RecordDate <= endDate)
            .Where(r => accountIds.Contains(r.AccountId))
            .OrderBy(r => r.RecordDate)
            .ToListAsync();

        return records;
    }

    private IQueryable<Record> GetRecordByIdBaseQuery(string userId, Guid recordId)
    {
        return GetAll()
            .Include(r => r.Account)
                .ThenInclude(a => a.Currency)
            .Include(r => r.FromAccount)
                .ThenInclude(a => a.Currency)
            .Include(r => r.PaymentType)
            .Include(r => r.Category)
            .Where(r => r.Account.UserId == userId)
            .Where(r => r.Id == recordId);
    }

    private IQueryable<Record> GetAllBaseQuery(string userId)
    {
        return GetAll()
            .Include(r => r.Account)
                .ThenInclude(a => a.Currency)
            .Include(r => r.FromAccount)
            .Include(r => r.PaymentType)
            .Include(r => r.Category)
            .Where(r => r.Account.UserId == userId)
            .OrderByDescending(r => r.RecordDate);
    }
}
