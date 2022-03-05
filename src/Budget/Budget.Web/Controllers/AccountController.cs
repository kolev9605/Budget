using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Budget.Web.Controllers
{
    [Authorize]
    [Route("Account")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create(CreateAccountModel createAccountModel)
            => Ok(await _accountService.CreateAccountAsync(createAccountModel, LoggedInUserId));

        [HttpGet]
        [Route(nameof(Delete))]
        public async Task<IActionResult> Delete(int accountId)
            => Ok(await _accountService.DeleteAccountAsync(accountId));

        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
            => Ok(await _accountService.GetAllAccountsAsync(LoggedInUserId));

        [HttpGet]
        [Route(nameof(GetById))]
        public async Task<IActionResult> GetById(int accountId)
            => Ok(await _accountService.GetByIdAsync(accountId));

        [HttpPost]
        [Route(nameof(Update))]
        public async Task<IActionResult> Update(UpdateAccountModel updateAccountModel)
            => Ok(await _accountService.UpdateAsync(updateAccountModel));
    }
}
