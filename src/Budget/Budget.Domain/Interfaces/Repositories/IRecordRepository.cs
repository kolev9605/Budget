using Budget.Domain.Entities;
using Budget.Domain.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces.Repositories
{
    public interface IRecordRepository : IRepository<Record>
    {
        Task<TResult> GetRecordByIdAsync<TResult>(int id, string userId);

        Task<TResult> GetPositiveTransferRecordAsync<TResult>(DateTime recordDate, int categoryId, decimal recordAmount);

        Task<TResult> GetNegativeTransferRecordAsync<TResult>(Record record);

        Task<IEnumerable<TResult>> GetAllAsync<TResult>(string userId);

        //Task<IDictionary<TAccountResult, IEnumerable<TRecordResult>>> GetRecordsGroupedByAccount<TAccountResult, TRecordResult>(string userId);

        Task<IPagedListContainer<TResult>> GetAllPaginatedAsync<TResult>(string userId, PaginatedRequestModel queryStringParameters);

        Task<IEnumerable<TResult>> GetAllInRangeAndAccountsAsync<TResult>(string userId, DateTime startDate, DateTime endDate, IEnumerable<int> accountIds);

        Task<IEnumerable<TResult>> GetAllInRangeAsync<TResult>(string userId, DateTime startDate, DateTime endDate);
    }
}
