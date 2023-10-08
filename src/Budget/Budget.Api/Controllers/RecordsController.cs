using Budget.Common;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Pagination;
using Budget.Domain.Models.Records;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Api.Controllers;

public class RecordsController : BaseController
{
    private readonly IRecordService _recordService;

    public RecordsController(IRecordService recordService)
    {
        _recordService = recordService;
    }

    [HttpGet]
    [Route(nameof(GetById))]
    public async Task<IActionResult> GetById(int recordId)
        => Ok(await _recordService.GetByIdAsync(recordId, LoggedInUserId));

    [HttpGet]
    [Route(nameof(GetByIdForUpdate))]
    public async Task<IActionResult> GetByIdForUpdate(int recordId)
        => Ok(await _recordService.GetByIdForUpdateAsync(recordId, LoggedInUserId));

    [HttpGet]
    [Route(nameof(GetAllPaginated))]
    public async Task<ActionResult<IEnumerable<RecordModel>>> GetAllPaginated([FromQuery] PaginatedRequestModel requestModel)
         => Ok(await _recordService.GetAllPaginatedAsync(requestModel, LoggedInUserId));

    [HttpPost]
    [Route(nameof(Create))]
    public async Task<IActionResult> Create(CreateRecordModel createRecordModel)
        => Ok(await _recordService.CreateAsync(createRecordModel, LoggedInUserId));

    [HttpPost]
    [Route(nameof(Update))]
    public async Task<IActionResult> Update(UpdateRecordModel updateRecordModel)
        => Ok(await _recordService.UpdateAsync(updateRecordModel, LoggedInUserId));

    [HttpDelete]
    [Route(nameof(Delete))]
    public async Task<IActionResult> Delete(int recordId)
        => Ok(await _recordService.DeleteAsync(recordId, LoggedInUserId));

    [HttpGet]
    [Route(nameof(GetRecordTypes))]
    public IActionResult GetRecordTypes()
        => Ok(EnumHelpers.GetListFromEnum<RecordType>());

    [HttpGet]
    [Route(nameof(GetRecordsDateRange))]
    public async Task<IActionResult> GetRecordsDateRange()
        => Ok(await _recordService.GetRecordsDateRangeAsync(LoggedInUserId));
}
