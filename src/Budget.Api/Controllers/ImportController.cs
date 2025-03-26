using Budget.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Budget.Api.Controllers;

public class ImportController : BaseController
{
    private readonly IImportService _importService;

    public ImportController(IImportService importService)
    {
        _importService = importService;
    }

    [HttpPost]
    [Route(nameof(ImportRecords))]
    public async Task<IActionResult> ImportRecords()
    {
        var file = Request.Form.Files[0];
        string fileContents;
        using (var stream = file.OpenReadStream())
        using (var reader = new StreamReader(stream))
        {
            fileContents = await reader.ReadToEndAsync();
        }

        await _importService.ImportRecordsAsync(fileContents, CurrentUser.Id);

        return Ok();
    }

    [HttpPost]
    [Route(nameof(ImportWalletRecords))]
    public async Task<IActionResult> ImportWalletRecords()
    {
        var file = Request.Form.Files[0];
        string fileContents;
        using (var stream = file.OpenReadStream())
        using (var reader = new StreamReader(stream))
        {
            fileContents = await reader.ReadToEndAsync();
        }

        var recordsInserted = await _importService.ImportWalletRecordsAsync(fileContents, CurrentUser.Id);

        return Ok(recordsInserted);
    }
}
