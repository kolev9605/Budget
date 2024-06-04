using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Charts.CashFlow;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Budget.Api.Controllers;

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
        => Ok(await _chartService.GetCashFlowChartDataAsync(cashFlowChartRequestModel, CurrentUser.Id));
}
