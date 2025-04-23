using Budget.Domain.Entities;
using Budget.Domain.Models.Pagination;
using Budget.Domain.Models.Records;

namespace Budget.Domain.Interfaces.Repositories;

public interface IRecordRepository : IRepository<Record>
{
    Task<Record?> GetRecordByIdAsync(Guid id, string userId);

    Task<RecordModel?> GetRecordByIdMappedAsync(Guid id, string userId);

    Task<RecordModel?> GetPositiveTransferRecordMappedAsync(DateTimeOffset recordDate, Guid categoryId, decimal recordAmount);

    Task<Record?> GetNegativeTransferRecordAsync(Record record);

    Task<IEnumerable<Record>> GetAllAsync(string userId);

    Task<IEnumerable<RecordsExportModel>> GetAllForExportAsync(string userId);

    Task<IPagedListContainer<RecordModel>> GetAllPaginatedAsync(
        string userId,
        Guid? accountId,
        RecordType? recordType,
        Guid? categoryId,
        DateTime? referenceStartDate,
        DateTime? referenceEndDate,
        int pageNumber,
        int pageSize);

    Task<IEnumerable<Record>> GetAllInRangeAndAccountsAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate, IEnumerable<Guid> accountIds);

    Task<RecordsDateRangeResult?> GetDateRangeByUserAsync(string userId);
}
