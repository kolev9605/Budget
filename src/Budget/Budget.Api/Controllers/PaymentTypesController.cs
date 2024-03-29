﻿using Budget.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Api.Controllers;

public class PaymentTypesController : BaseController
{
    private readonly IPaymentTypeService _paymentTypeService;

    public PaymentTypesController(IPaymentTypeService paymentTypeService)
    {
        _paymentTypeService = paymentTypeService;
    }

    [HttpGet]
    [Route(nameof(GetAll))]
    public async Task<IActionResult> GetAll()
        => Ok(await _paymentTypeService.GetAllAsync());
}
