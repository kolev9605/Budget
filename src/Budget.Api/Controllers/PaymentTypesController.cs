using Budget.Api.Models.PaymentTypes;
using Budget.Application.PaymentTypes.Queries;
using Budget.Domain.Models.PaymentTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Api.Controllers;

public class PaymentTypesController : BaseController
{

    private readonly IMediator _mediator;
    public PaymentTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route(nameof(GetAll))]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllPaymentTypesQuery());

        return MatchResponse<IEnumerable<PaymentTypeModel>, IEnumerable<PaymentTypeResponse>>(result);
    }
}
