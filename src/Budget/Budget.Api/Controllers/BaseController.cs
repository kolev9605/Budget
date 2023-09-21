using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Budget.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public abstract class BaseController : ControllerBase
{
    protected string? LoggedInUserId
    {
        get
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
