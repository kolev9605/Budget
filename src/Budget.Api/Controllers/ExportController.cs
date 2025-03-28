using Budget.Api.Models.Exports;
using Budget.Application.Exports.Queries;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Api.Controllers;

// TODO: Is this feature even needed? Drop it?
public class ExportController : BaseController
{
    private readonly IMediator _mediator;
    public ExportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route(nameof(ExportRecords))]
    public async Task<IActionResult> ExportRecords()
    {
        var result = await _mediator.Send(CurrentUser.Adapt<ExportRecordsQuery>());

        // TODO: Clean this logic - figure out how to move it in the base controller
        return result.Match(
            value => File(value.Adapt<ExportRecordsResponse>().Bytes, "application/json", "file.json"),
            errors => Problem(errors)
        );
    }
}
