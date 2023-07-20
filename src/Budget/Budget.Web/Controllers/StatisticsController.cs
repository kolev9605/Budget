using Budget.Application.Interfaces.Services;
using Budget.Application.Models.Statistics;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Web.Controllers
{
    public class StatisticsController : BaseController
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpPost]
        [Route(nameof(GetStatistics))]
        public async Task<IActionResult> GetStatistics(StatisticsRequestModel statisticsRequestModel)
            => Ok(await _statisticsService.GetStatisticsByDateAsync(statisticsRequestModel, LoggedInUserId));
    }
}
