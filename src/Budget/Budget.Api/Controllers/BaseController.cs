using Budget.Api.Models;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Budget.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public abstract class BaseController : ControllerBase
{
    private string? LoggedInUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    private string? LoggedInUserEmail => User.FindFirstValue(ClaimTypes.Email);


    /// <summary>
    /// Returns a AuthenticatedUserModel which data is extracted from the JWT Token.
    /// </summary>
    protected AuthenticatedUserModel CurrentUser
    {
        get
        {
            if (LoggedInUserId is not null &&
                LoggedInUserEmail is not null)
            {
                return new AuthenticatedUserModel(
                    LoggedInUserId,
                    LoggedInUserEmail);
            }

            throw new UnauthorizedAccessException();
        }
    }

    protected IActionResult MatchResponse<TResult, TResponse>(ErrorOr<TResult> result)
    {
        return result.Match(
            value => Ok(value.Adapt<TResponse>()),
            errors => Problem(errors)
        );
    }

    private IActionResult Problem(List<Error> errors)
    {
        var firstError = errors[0];

        var statusCode = firstError.Type switch
        {
            ErrorType.NotFound => HttpStatusCode.NotFound,
            ErrorType.Validation => HttpStatusCode.BadRequest,
            ErrorType.Unauthorized => HttpStatusCode.Unauthorized,
            ErrorType.Forbidden => HttpStatusCode.Forbidden,
            _ => HttpStatusCode.InternalServerError
        };

        return Problem(statusCode: (int)statusCode, title: firstError.Description);
    }
}
