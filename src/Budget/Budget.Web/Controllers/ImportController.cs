using Budget.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Budget.Web.Controllers
{
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

            await _importService.ImportRecordsAsync(fileContents, LoggedInUserId);

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

            var recordsInserted = await _importService.ImportWalletRecordsAsync(fileContents, LoggedInUserId);

            return Ok(recordsInserted);
        }
    }
}
