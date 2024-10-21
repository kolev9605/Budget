using Budget.Api.Models.Statistics;
using Budget.Application.Statistics.Queries;
using Budget.Domain.Models.Statistics;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Api.Controllers;

public class StatisticsController : BaseController
{
    private readonly IMediator _mediator;

    public StatisticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route(nameof(GetCashFlow))]
    public async Task<IActionResult> GetCashFlow(GetCashFlowStatisticsRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<GetCashFlowStatisticsQuery>());

        return MatchResponse<GetCashFlowStatisticsResult, GetCashFlowStatisticsResponse>(result);
    }
}
