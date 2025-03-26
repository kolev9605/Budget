using Budget.Api.Models.Records;
using Budget.Application.Records.Commands;
using Budget.Application.Records.Queries;
using Budget.Common;
using Budget.Domain.Entities;
using Budget.Domain.Models.Pagination;
using Budget.Domain.Models.Records;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Api.Controllers;

public class RecordsController : BaseController
{
    private readonly IMediator _mediator;

    public RecordsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route(nameof(GetById))]
    public async Task<IActionResult> GetById([FromQuery] GetRecordByIdRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<GetRecordByIdQuery>());

        return MatchResponse<RecordModel, RecordResponse>(result);
    }

    [HttpGet]
    [Route(nameof(GetByIdForUpdate))]
    public async Task<IActionResult> GetByIdForUpdate([FromQuery] GetRecordByIdForUpdateRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<GetRecordByIdForUpdateQuery>());

        return MatchResponse<RecordModel, RecordResponse>(result);
    }

    [HttpGet]
    [Route(nameof(GetAllPaginated))]
    public async Task<IActionResult> GetAllPaginated([FromQuery] GetAllRecordsRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<GetAllRecordsQuery>());

        return MatchResponse<IPagedListContainer<RecordModel>, IPagedListContainer<RecordResponse>>(result);
    }

    [HttpPost]
    [Route(nameof(Create))]
    public async Task<IActionResult> Create(CreateRecordRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<CreateRecordCommand>());

        return MatchResponse<RecordModel, RecordResponse>(result);
    }

    [HttpPut]
    [Route(nameof(Update))]
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

    [HttpGet]
    [Route(nameof(GetRecordTypes))]
    public IActionResult GetRecordTypes()
        => Ok(EnumHelpers.GetListFromEnum<RecordType>());

    [HttpGet]
    [Route(nameof(GetRecordsDateRange))]
    public async Task<IActionResult> GetRecordsDateRange()
    {
        var result = await _mediator.Send(CurrentUser.Adapt<GetRecordsDateRangeQuery>());

        return MatchResponse(result);
    }
}
