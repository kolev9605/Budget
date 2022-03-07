using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Budget.Core.Entities;
using Budget.Common;

namespace Budget.Web.Controllers
{
    [Authorize]
    [Route("Records")]
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
            => Ok(await _recordService.GetByIdAsync(recordId));

        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<ActionResult<IEnumerable<RecordModel>>> GetAll()
            => Ok(await _recordService.GetAllAsync());

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create(CreateRecordModel createRecordModel)
            => Ok(await _recordService.CreateAsync(createRecordModel, LoggedInUserId));

        [HttpPost]
        [Route(nameof(Update))]
        public async Task<IActionResult> Update(UpdateRecordModel updateRecordModel)
            => Ok(await _recordService.UpdateAsync(updateRecordModel));

        [HttpGet]
        [Route(nameof(Delete))]
        public async Task<IActionResult> Delete(int recordId)
            => Ok(await _recordService.DeleteAsync(recordId));

        [HttpGet]
        [Route(nameof(GetRecordTypes))]
        public IActionResult GetRecordTypes()
            => Ok(EnumHelpers.GetListFromEnum<RecordType>());
    }
}
