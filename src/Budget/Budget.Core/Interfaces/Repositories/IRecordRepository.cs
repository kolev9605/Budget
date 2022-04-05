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

        Task<PaginationModel<Record>> GetAllPaginatedAsync(string userId, QueryStringParameters queryStringParameters);

        Task<IEnumerable<Record>> GetAllByMonthAndAccountsAsync(string userId, DateTime startDate, DateTime endDate, IEnumerable<int> accountIds);
    }
}
