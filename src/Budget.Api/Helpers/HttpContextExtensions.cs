using System.Net;
using System.Security.Claims;
using Budget.Api.Models;
using ErrorOr;

namespace Budget.Api.Helpers;

public static class HttpContextExtensions
{
    public static AuthenticatedUserModel GetCurrentUser(this HttpContext httpContext)
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userEmail = httpContext.User.FindFirstValue(ClaimTypes.Email);

        if (userId is not null && userEmail is not null)
        {
            return new AuthenticatedUserModel(userId, userEmail);
        }

        throw new UnauthorizedAccessException();
    }

    public static IResult MatchResponse<TResult>(this ErrorOr<TResult> result)
    {
        return result.Match(
            value => Results.Ok(value),
            errors => HandleErrors(errors)
        );
    }

    private static IResult HandleErrors(List<Error> errors)
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

        return Results.Problem(
            statusCode: (int)statusCode,
            title: firstError.Description);
    }
}
