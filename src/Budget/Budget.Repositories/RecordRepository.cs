using Budget.Core.Entities;
using Budget.Core.Interfaces.Repositories;
using Budget.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Budget.Repositories
{
    public class RecordRepository : Repository<Record>, IRecordRepository
    {
        public RecordRepository(BudgetDbContext budgetDbContext) : base(budgetDbContext)
        {
        }

        public async Task<IEnumerable<Record>> GetAllWithCurrenciesAsync()
        {
            var records = await _budgetDbContext.Records
                .Include(r => r.Currency)
                .ToListAsync();

            return records;
        }
    }
}
