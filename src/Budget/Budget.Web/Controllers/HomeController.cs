using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Web.Controllers
{
    [AllowAnonymous]
    [Route("")]
    public class HomeController : BaseController
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
            => Ok("Hello from Budget! CI/CD Working?!");
    }
}
