using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Budget.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create(CreateAccountModel createAccountModel)
            => Ok(await _accountService.CreateAccountAsync(createAccountModel, User.FindFirstValue(ClaimTypes.NameIdentifier)));

        [HttpGet]
        [Route(nameof(Delete))]
        public async Task<IActionResult> Delete(int accountId)
            => Ok(await _accountService.DeleteAccountAsync(accountId));

        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
            => Ok(await _accountService.GetAllAccountsAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
    }
}
