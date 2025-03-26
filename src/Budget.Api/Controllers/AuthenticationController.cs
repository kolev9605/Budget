using Budget.Api.Models.Authentication;
using Budget.Application.Authentication.Commands;
using Budget.Application.Authentication.Queries;
using Budget.Domain.Models.Authentication;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Api.Controllers;

[AllowAnonymous]
public class AuthenticationController(
    IMediator _mediator) : BaseController
{
    [HttpPost]
    [Route(nameof(Login))]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var result = await _mediator.Send(loginRequest.Adapt<LoginQuery>());

        return MatchResponse<AuthenticationResult, AuthenticationResponse>(result);
    }

    [HttpPost]
    [Route(nameof(Register))]
    public async Task<IActionResult> Register(RegistrationRequest registrationRequest)
    {
        var result = await _mediator.Send(registrationRequest.Adapt<RegistrationCommand>());

        return MatchResponse<RegistrationResult, RegistrationResponse>(result);
    }
}
