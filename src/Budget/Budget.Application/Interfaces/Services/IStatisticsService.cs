using Budget.Application.Models.Statistics;
using System.Threading.Tasks;

namespace Budget.Application.Interfaces.Services
{
    public interface IStatisticsService
    {
        Task<StatisticsResultModel> GetStatisticsByDateAsync(StatisticsRequestModel statisticsRequestModel, string userId);
    }
}
