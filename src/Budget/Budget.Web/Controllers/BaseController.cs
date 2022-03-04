using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Budget.Web.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected string LoggedInUserToken
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }
    }
}
