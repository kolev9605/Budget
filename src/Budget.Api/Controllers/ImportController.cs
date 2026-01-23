using Budget.Application.Import.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Api.Controllers;

public class ImportController : BaseController
{
    private readonly IMediator _mediator;

    public ImportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route(nameof(ImportWalletRecords))]
    public async Task<IActionResult> ImportWalletRecords()
    {
        if (Request.Form.Files.Count == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var file = Request.Form.Files[0];
        string fileContents;
        using (var stream = file.OpenReadStream())
        using (var reader = new StreamReader(stream))
        {
            fileContents = await reader.ReadToEndAsync();
        }

        var importedRecords = await _mediator.Send(new ImportWalletRecordsCommand(fileContents, CurrentUser.Id));

        return Ok(importedRecords);
    }
}
