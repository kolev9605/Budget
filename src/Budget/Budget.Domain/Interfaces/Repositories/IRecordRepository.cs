using Budget.Domain.Entities;
using Budget.Domain.Models.Pagination;
using Budget.Domain.Models.Records;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces.Repositories;

public interface IRecordRepository : IRepository<Record>
{
    Task<Record> GetRecordByIdAsync(int id, string userId);

    Task<RecordModel> GetRecordByIdMappedAsync(int id, string userId);

    Task<RecordModel> GetPositiveTransferRecordMappedAsync(DateTime recordDate, int categoryId, decimal recordAmount);

    Task<Record> GetNegativeTransferRecordAsync(Record record);

    Task<IEnumerable<Record>> GetAllAsync(string userId);

    Task<IEnumerable<RecordsExportModel>> GetAllForExportAsync(string userId);

    //Task<IDictionary<TAccountResult, IEnumerable<TRecordResult>>> GetRecordsGroupedByAccount<TAccountResult, TRecordResult>(string userId);

    Task<IPagedListContainer<RecordModel>> GetAllPaginatedAsync(string userId, PaginatedRequestModel queryStringParameters);

    Task<IEnumerable<Record>> GetAllInRangeAndAccountsAsync(string userId, DateTime startDate, DateTime endDate, IEnumerable<int> accountIds);
}
