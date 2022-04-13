using Budget.Core.Entities;
using Budget.Core.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Repositories
{
    public interface IRecordRepository : IRepository<Record>
    {
        Task<Record> GetRecordByIdAsync(int id, string userId);

        Task<Record> GetPositiveTransferRecordAsync(Record record);

        Task<Record> GetNegativeTransferRecordAsync(Record record);

        Task<IEnumerable<Record>> GetAllAsync(string userId);

        Task<PaginationModel<Record>> GetAllPaginatedAsync(string userId, PaginatedRequestModel queryStringParameters);

        Task<IEnumerable<Record>> GetAllInRangeAndAccountsAsync(string userId, DateTime startDate, DateTime endDate, IEnumerable<int> accountIds);

        Task<IEnumerable<Record>> GetAllInRangeAsync(string userId, DateTime startDate, DateTime endDate);
    }
}
