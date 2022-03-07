using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Budget.Web.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected string LoggedInUserId
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }
    }
}
