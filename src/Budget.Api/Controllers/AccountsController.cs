using Budget.Api.Models.Accounts;
using Budget.Application.Accounts.Commands;
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
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var getAccountByIdRequest = new GetAccountByIdRequest(id);
        var result = await _mediator.Send((getAccountByIdRequest, CurrentUser).Adapt<GetAccountByIdQuery>());

        return MatchResponse<AccountModel, AccountResponse>(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(CurrentUser.Adapt<GetAllAccountsQuery>());

        return MatchResponse<IEnumerable<AccountModel>, IEnumerable<AccountResponse>>(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAccountRequest createAccountRequest)
    {
        var result = await _mediator.Send((createAccountRequest, CurrentUser).Adapt<CreateAccountCommand>());

        return MatchResponse<AccountModel, AccountResponse>(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateAccountRequest updateAccountRequest)
    {
        var result = await _mediator.Send((updateAccountRequest, CurrentUser).Adapt<UpdateAccountCommand>());

        return MatchResponse<AccountModel, AccountResponse>(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new DeleteAccountCommand(id, CurrentUser.Id));

        return MatchResponse<AccountModel, AccountResponse>(result);
    }
}
