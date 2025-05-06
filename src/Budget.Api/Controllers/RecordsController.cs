using Budget.Api.Models.Records;
using Budget.Application.Records.Commands;
using Budget.Application.Records.Queries;
using Budget.Application.Records.Queries.AiGenerate;
using Budget.Common;
using Budget.Domain.Entities;
using Budget.Domain.Models.Pagination;
using Budget.Domain.Models.Records;
using Budget.Domain.Models.Records.Statistics;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Api.Controllers;

public class RecordsController : BaseController
{
    private readonly IMediator _mediator;

    public RecordsController(
        IMediator mediator
        )
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var request = new GetRecordByIdRequest(id);
        var result = await _mediator.Send((request, CurrentUser).Adapt<GetRecordByIdQuery>());

        return MatchResponse<RecordModel, RecordResponse>(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPaginated([FromQuery] GetAllRecordsRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<GetAllRecordsQuery>());

        return MatchResponse<IPagedListContainer<RecordModel>, IPagedListContainer<RecordResponse>>(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRecordRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<CreateRecordCommand>());

        return MatchResponse<RecordModel, RecordResponse>(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateRecordRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<UpdateRecordCommand>());

        return MatchResponse<RecordModel, RecordResponse>(result);
    }

    [HttpDelete]
    [Route(nameof(Delete))]
    public async Task<IActionResult> Delete([FromQuery] DeleteRecordRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<DeleteRecordCommand>());

        return MatchResponse<RecordModel, RecordResponse>(result);
    }

    [HttpGet("Types")]
    public IActionResult GetRecordTypes()
        => Ok(EnumHelpers.GetListFromEnum<RecordType>());

    [HttpGet]
    [Route(nameof(GetRecordsDateRange))]
    public async Task<IActionResult> GetRecordsDateRange()
    {
        var result = await _mediator.Send(CurrentUser.Adapt<GetRecordsDateRangeQuery>());

        return MatchResponse(result);
    }

    [HttpGet("totalbalance")]
    public async Task<IActionResult> GetTotalBalance()
    {
        var result = await _mediator.Send(new GetTotalBalanceQuery(CurrentUser.Id));

        return MatchResponse<decimal, decimal>(result);
    }

    [HttpGet("statistics")]
    public async Task<IActionResult> GetCashFlow([FromQuery] GetRecordsStatisticsRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<GetRecordsStatisticsQuery>());

        return MatchResponse<IEnumerable<GetRecordsStatisticsResult>, IEnumerable<GetRecordsStatisticsResponse>>(result);
    }

    [HttpPost("ai-generate")]
    public async Task<IActionResult> AiGenerate([FromBody] string prompt)
    {
        var result = await _mediator.Send(new AiGenerateRecordQuery(prompt, CurrentUser.Id));
        await Task.Delay(1000); // Simulate some delay for the AI generation

        return MatchResponse<AiGenerateRecordQueryResult, AiGenerateRecordQueryResult>(result);
    }
}
