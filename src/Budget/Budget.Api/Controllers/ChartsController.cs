using Budget.Api.Models.Charts;
using Budget.Application.Charts.Queries.GetCashFlowChart;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Charts.CashFlow;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Api.Controllers;

public class ChartsController : BaseController
{
    private readonly IMediator _mediator;

    public ChartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route(nameof(GetCashFlowData))]
    public async Task<IActionResult> GetCashFlowData(GetCashFlowChartRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<GetCashFlowChartQuery>());

        return MatchResponse<CashFlowChartModel, CashFlowChartResponse>(result);
    }
}
