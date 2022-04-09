using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Web.Controllers
{
    [Authorize(Roles = Roles.Administrator)]
    [Route("Admin")]
    public class AdminController : BaseController
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route(nameof(GetUsers))]
        public async Task<IActionResult> GetUsers()
            => Ok(await _userService.GetUsersAsync());
    }
}
