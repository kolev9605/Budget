using Budget.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Budget.Web.Controllers
{
    [Route("Import")]
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

            await _importService.ImportRecords(fileContents, LoggedInUserId);

            return Ok();
        }
    }
}
