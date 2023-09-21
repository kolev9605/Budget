using Budget.Domain.Constants;
using Budget.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Api.Controllers;

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
