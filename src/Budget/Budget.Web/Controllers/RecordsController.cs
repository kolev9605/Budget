using Budget.Core.Entities;
using Budget.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Web.Controllers
{
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
        public async Task<IEnumerable<Record>> GetAll()
        {
            var records = await _recordService.GetAllAsync();
            return records;
        }
    }
}
