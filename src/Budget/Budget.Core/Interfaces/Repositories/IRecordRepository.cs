using Budget.Core.Entities;
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

        Task<IEnumerable<Record>> GetAllByMonthAsync(string userId, int month);
    }
}
