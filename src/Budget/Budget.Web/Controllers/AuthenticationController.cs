using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Web.Controllers
{
    [ApiController]
    [Route("Authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var token = await _userService.LoginAsync(model);

            return Ok(token);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            await _userService.RegisterAsync(model);

            return Ok();
        }
    }
}
