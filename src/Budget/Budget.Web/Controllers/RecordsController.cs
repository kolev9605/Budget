using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Authentication;
using Budget.Core.Models.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Web.Controllers
{
    
    [ApiController]
    [Route("Records")]
    public class RecordsController : ControllerBase
    {
        private readonly IRecordService _recordService;

        public RecordsController(IRecordService recordService)
        {
            _recordService = recordService;
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<ActionResult<IEnumerable<RecordModel>>> GetAll()
        {
            var records = await _recordService.GetAllAsync();
            return Ok(records);
        }
    }
}
