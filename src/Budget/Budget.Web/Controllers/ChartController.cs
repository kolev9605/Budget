using Budget.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Budget.Web.Controllers
{
    [Authorize]
    [Route("Chart")]
    public class ChartController : BaseController
    {
        private readonly IChartService _chartService;

        public ChartController(IChartService chartService)
        {
            _chartService = chartService;
        }

        [HttpGet]
        [Route(nameof(GetCashFlowData))]
        public async Task<IActionResult> GetCashFlowData(int month)
            => Ok(await _chartService.GetCashFlowChartData(LoggedInUserId, month));
    }
}
