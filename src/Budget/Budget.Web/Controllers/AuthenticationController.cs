using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Web.Controllers
{
    [Route("Authentication")]
    public class AuthenticationController : BaseController
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
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
