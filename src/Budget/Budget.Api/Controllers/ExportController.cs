using Budget.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Budget.Api.Controllers;

public class ExportController : BaseController
{
    private readonly IRecordService _recordService;
    private readonly IExportService _exportService;

    public ExportController(IRecordService recordService, IExportService exportService)
    {
        _recordService = recordService;
        _exportService = exportService;
    }

    [HttpGet]
    [Route(nameof(ExportRecords))]
    public async Task<IActionResult> ExportRecords()
    {
        var records = await _recordService.GetAllForExportAsync(CurrentUser.Id);

        var recordsByteArray = _exportService.SerializeToByteArray(records, new JsonSerializerSettings());

        return File(recordsByteArray, "application/json", "file.json");
    }
}
