using Budget.Core.Entities;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Statistics;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Infrastructure.Services
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
            var recordsInRange = await _recordRepository.GetAllInRangeAsync(userId, statisticsRequestModel.StartDate, statisticsRequestModel.EndDate);

            var income = recordsInRange
                .Where(r => r.RecordType == RecordType.Income)
                .Sum(r => r.Amount);

            var expense = recordsInRange
                .Where(r => r.RecordType == RecordType.Expense)
                .Sum(r => r.Amount);

            var resultModel = new StatisticsResultModel(income, expense);

            return resultModel;
        }
    }
}
