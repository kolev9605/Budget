using Budget.Api.Models.Accounts;
using Budget.Application.Accounts.Commands.Create;
using Budget.Application.Accounts.Queries.GetAll;
using Budget.Application.Accounts.Queries.GetById;
using Budget.Domain.Models.Accounts;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Api.Controllers;

public class AccountsController(
    IMediator _mediator) : BaseController
{
    [HttpGet]
    [Route(nameof(GetById))]
    public async Task<IActionResult> GetById([FromQuery] GetAccountByIdRequest getAccountByIdRequest)
    {
        var result = await _mediator.Send((getAccountByIdRequest, CurrentUser).Adapt<GetAccountByIdQuery>());

        return MatchResponse<AccountModel, AccountResponse>(result);
    }

    [HttpGet]
    [Route(nameof(GetAll))]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(CurrentUser.Adapt<GetAllAccountsQuery>());

        return MatchResponse<IEnumerable<AccountModel>, IEnumerable<AccountResponse>>(result);
    }

    [HttpPost]
    [Route(nameof(Create))]
    public async Task<IActionResult> Create(CreateAccountRequest createAccountRequest)
    {
        var result = await _mediator.Send((createAccountRequest, CurrentUser).Adapt<CreateAccountCommand>());

        return MatchResponse<AccountModel, AccountResponse>(result);
    }

    [HttpPut]
    [Route(nameof(Update))]
    public async Task<IActionResult> Update(UpdateAccountRequest updateAccountRequest)
    {
        var result = await _mediator.Send((updateAccountRequest, CurrentUser).Adapt<UpdateAccountCommand>());

        return MatchResponse<AccountModel, AccountResponse>(result);
    }

    [HttpDelete]
    [Route(nameof(Delete))]
    public async Task<IActionResult> Delete([FromQuery] DeleteAccountRequest deleteAccountRequest)
    {
        var result = await _mediator.Send((deleteAccountRequest, CurrentUser).Adapt<DeleteAccountCommand>());

        return MatchResponse<AccountModel, AccountResponse>(result);
    }
}
