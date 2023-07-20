using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Web.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IImportService _importService;

        public AuthenticationController(IAuthenticationService authenticationService, IImportService importService)
        {
            _authenticationService = authenticationService;
            _importService = importService;
        }

        [HttpPost]
        [Route(nameof(Login))]
        public async Task<IActionResult> Login(LoginModel model)
            => Ok(await _authenticationService.LoginAsync(model));

        [HttpPost]
        [Route(nameof(Register))]
        public async Task<IActionResult> Register(RegisterModel model)
            => Ok(await _authenticationService.RegisterAsync(model));
    }
}
