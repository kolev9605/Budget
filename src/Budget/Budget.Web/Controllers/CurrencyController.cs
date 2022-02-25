using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Web.Controllers
{
    [Authorize]
    [ApiController]
    public class CurrencyController : ControllerBase
    {


        //public IActionResult GetAll()
        //{
        //    return Ok();
        //}
    }
}
