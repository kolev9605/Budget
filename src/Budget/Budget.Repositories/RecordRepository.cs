using Budget.Core.Entities;
using Budget.Core.Interfaces.Repositories;
using Budget.Persistance;
using Microsoft.EntityFrameworkCore;
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
                .Include(r => r.PaymentType)
                .Include(r => r.Category)
                .Where(r => r.Account.UserId == userId)
                .FirstOrDefaultAsync(r => r.Id == id);

            return record;
        }

        public async Task<IEnumerable<Record>> GetAllAsync(string userId)
        {
            var records = await _budgetDbContext.Records
                .Include(r => r.Account)
                    .ThenInclude(a => a.Currency)
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
