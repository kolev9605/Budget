using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var token = await _userService.LoginAsync(model);

            return Ok(token);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            await _userService.RegisterAsync(model);

            return Ok();
        
    }
}
