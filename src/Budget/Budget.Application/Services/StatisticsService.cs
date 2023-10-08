using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Statistics;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Application.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IRecordRepository _recordRepository;

        public StatisticsService(IRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }

        public async Task<StatisticsResultModel> GetStatisticsByDateAsync(StatisticsRequestModel statisticsRequestModel, string userId)
        {
            var recordsInRange = await _recordRepository.GetAllInRangeAndAccountsAsync(userId, statisticsRequestModel.StartDate, statisticsRequestModel.EndDate, statisticsRequestModel.AccountIds);

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
