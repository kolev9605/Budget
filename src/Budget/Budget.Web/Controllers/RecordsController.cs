using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("records")]
    public class RecordsController : ControllerBase
    {
        private readonly IRecordService _recordService;

        public RecordsController(IRecordService recordService)
        {
            _recordService = recordService;
        }

        [HttpGet]
        public async Task<IEnumerable<RecordModel>> GetAll()
        {
            var records = await _recordService.GetAllAsync();
            return records;
        }
    }
}
