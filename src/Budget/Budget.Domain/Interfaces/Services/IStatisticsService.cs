using Budget.Domain.Models.Statistics;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces.Services
{
    public interface IStatisticsService
    {
        Task<StatisticsResultModel> GetStatisticsByDateAsync(StatisticsRequestModel statisticsRequestModel, string userId);
    }
}
