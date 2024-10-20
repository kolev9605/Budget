using Budget.Api.Models.Currencies;
using Budget.Application.Currencies.Queries;
using Budget.Domain.Models.Currencies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Api.Controllers;

public class CurrenciesController : BaseController
{
    private readonly IMediator _mediator;

    public CurrenciesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route(nameof(GetAll))]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllCurrenciesQuery());

        return MatchResponse<IEnumerable<CurrencyModel>, IEnumerable<CurrencyResponse>>(result);
    }
}
