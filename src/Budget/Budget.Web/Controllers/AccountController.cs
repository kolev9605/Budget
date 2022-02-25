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
        {
            var id = await _accountService.CreateAccountAsync(createAccountModel, User.FindFirstValue(ClaimTypes.NameIdentifier));

            return Ok(id);
        }

        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _accountService.GetAllAccountsAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return Ok(accounts);
        }
    }
}
