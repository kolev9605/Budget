using Budget.Application.Interfaces.Services;
using Budget.Application.Models.Charts.CashFlow;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Budget.Web.Controllers
{
    public class ChartController : BaseController
    {
        private readonly IChartService _chartService;

        public ChartController(IChartService chartService)
        {
            _chartService = chartService;
        }

        [HttpPost]
        [Route(nameof(GetCashFlowData))]
        public async Task<IActionResult> GetCashFlowData(CashFlowChartRequestModel cashFlowChartRequestModel)
            => Ok(await _chartService.GetCashFlowChartDataAsync(cashFlowChartRequestModel, LoggedInUserId));
    }
}
