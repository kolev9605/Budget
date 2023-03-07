using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Web.Controllers
{
    [AllowAnonymous]
    public class TestController : BaseController
    {
        [HttpGet]
        [Route(nameof(Hello))]
        public IActionResult Hello()
            => Ok("Hello v1!");
    }
}
