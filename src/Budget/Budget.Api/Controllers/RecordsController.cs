using Budget.Common;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Pagination;
using Budget.Domain.Models.Records;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> GetById(Guid recordId)
        => Ok(await _recordService.GetByIdAsync(recordId, CurrentUser.Id));

    [HttpGet]
    [Route(nameof(GetByIdForUpdate))]
    public async Task<IActionResult> GetByIdForUpdate(Guid recordId)
        => Ok(await _recordService.GetByIdForUpdateAsync(recordId, CurrentUser.Id));

    [HttpGet]
    [Route(nameof(GetAllPaginated))]
    public async Task<ActionResult<IEnumerable<RecordModel>>> GetAllPaginated([FromQuery] PaginatedRequestModel requestModel)
         => Ok(await _recordService.GetAllPaginatedAsync(requestModel, CurrentUser.Id));

    [HttpPost]
    [Route(nameof(Create))]
    public async Task<IActionResult> Create(CreateRecordModel createRecordModel)
        => Ok(await _recordService.CreateAsync(createRecordModel, CurrentUser.Id));

    [HttpPost]
    [Route(nameof(Update))]
    public async Task<IActionResult> Update(UpdateRecordModel updateRecordModel)
        => Ok(await _recordService.UpdateAsync(updateRecordModel, CurrentUser.Id));

    [HttpDelete]
    [Route(nameof(Delete))]
    public async Task<IActionResult> Delete(Guid recordId)
        => Ok(await _recordService.DeleteAsync(recordId, CurrentUser.Id));

    [HttpGet]
    [Route(nameof(GetRecordTypes))]
    public IActionResult GetRecordTypes()
        => Ok(EnumHelpers.GetListFromEnum<RecordType>());

    [HttpGet]
    [Route(nameof(GetRecordsDateRange))]
    public async Task<IActionResult> GetRecordsDateRange()
        => Ok(await _recordService.GetRecordsDateRangeAsync(CurrentUser.Id));
}
