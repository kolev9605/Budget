using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Web.Controllers
{
    [AllowAnonymous]
    [Route("Authentication")]
    public class AuthenticationController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IImportService _importService;

        public AuthenticationController(IUserService userService, IImportService importService)
        {
            _userService = userService;
            _importService = importService;
        }

        [HttpPost]
        [Route(nameof(Login))]
        public async Task<IActionResult> Login(LoginModel model)
            => Ok(await _userService.LoginAsync(model));

        [HttpPost]
        [Route(nameof(Register))]
        public async Task<IActionResult> Register(RegisterModel model)
            => Ok(await _userService.RegisterAsync(model));
    }
}
