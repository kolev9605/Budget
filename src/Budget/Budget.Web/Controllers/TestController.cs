using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Web.Controllers
{
    [AllowAnonymous]
    [Route("Statistics")]
    public class TestController : BaseController
    {
        [HttpGet]
        [Route(nameof(Hello))]
        public IActionResult Hello()
            => Ok("I am working");
    }
}
