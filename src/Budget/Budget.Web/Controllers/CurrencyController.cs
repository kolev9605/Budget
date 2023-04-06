using Budget.Application.Interfaces.Services;
using Budget.Core.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Web.Controllers
{
    public class CurrencyController : BaseController
    {
        private readonly ICurrencyService _currencyService;
        private readonly ICacheManager _cacheManager;

        public CurrencyController(ICurrencyService currencyService, ICacheManager cacheManager)
        {
            _currencyService = currencyService;
            _cacheManager = cacheManager;
        }

        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
            => Ok(await _cacheManager.GetOrCreateAsync(
                CacheConstants.Keys.Currencies, 
                CacheConstants.Expirations.CurrenciesExpirationInSeconds, 
                _currencyService.GetAllAsync));
    }
}
