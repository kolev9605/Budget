using Budget.Application.Interfaces;
using Budget.Application.Interfaces.Services;
using Budget.Application.Models.Statistics;
using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Infrastructure.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IBudgetDbContext _budgetDbContext;

        public StatisticsService(IBudgetDbContext budgetDbContext)
        {
            _budgetDbContext = budgetDbContext;
        }

        public async Task<StatisticsResultModel> GetStatisticsByDateAsync(StatisticsRequestModel statisticsRequestModel, string userId)
        {
            var recordsInRange = await _budgetDbContext.Records
                .Include(r => r.Account)
                .Where(r => r.Account.UserId == userId)
                .Where(r => r.RecordDate >= statisticsRequestModel.StartDate && r.RecordDate <= statisticsRequestModel.EndDate)
                .Where(r => statisticsRequestModel.AccountIds.Contains(r.AccountId))
                .OrderBy(r => r.RecordDate)
                .AsNoTracking()
                .ToListAsync();

            var income = recordsInRange
                .Where(r => r.Amount > 0)
                .Where(r => r.RecordType == RecordType.Income)
                .Sum(r => r.Amount);

            var expense = recordsInRange
                .Where(r => r.Amount < 0)
                .Where(r => r.RecordType == RecordType.Expense)
                .Sum(r => r.Amount);

            var resultModel = new StatisticsResultModel(income, expense);

            return resultModel;
        }
    }
}
