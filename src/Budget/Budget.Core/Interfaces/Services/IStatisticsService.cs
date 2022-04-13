using Budget.Core.Models.Statistics;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface IStatisticsService
    {
        Task<StatisticsResultModel> GetStatisticsByDateAsync(StatisticsRequestModel statisticsRequestModel, string userId);
    }
}
