﻿using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Accounts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Api.Controllers;

public class AccountController : BaseController
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    [Route(nameof(GetById))]
    public async Task<IActionResult> GetById(int accountId)
        => Ok(await _accountService.GetByIdAsync(accountId, LoggedInUserId));

    [HttpGet]
    [Route(nameof(GetAll))]
    public async Task<IActionResult> GetAll()
        => Ok(await _accountService.GetAllAccountsAsync(LoggedInUserId));

    [HttpPost]
    [Route(nameof(Create))]
    public async Task<IActionResult> Create(CreateAccountModel createAccountModel)
        => Ok(await _accountService.CreateAccountAsync(createAccountModel, LoggedInUserId));

    [HttpPost]
    [Route(nameof(Update))]
    public async Task<IActionResult> Update(UpdateAccountModel updateAccountModel)
        => Ok(await _accountService.UpdateAsync(updateAccountModel, LoggedInUserId));

    [HttpDelete]
    [Route(nameof(Delete))]
    public async Task<IActionResult> Delete(int accountId)
        => Ok(await _accountService.DeleteAccountAsync(accountId, LoggedInUserId));
}
