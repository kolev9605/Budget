using Budget.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Web.Controllers
{
    [Authorize]
    [Route("Currency")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {

        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
        {
            var currencies = await _currencyService.GetAllAsync();

            return Ok(currencies);
        }
    }
}
