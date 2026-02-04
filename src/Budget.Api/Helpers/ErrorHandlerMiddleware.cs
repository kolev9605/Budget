using System.Net.Mime;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Budget.Api.Helpers;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger, ProblemDetailsFactory problemDetailsFactory)
    {
        _next = next;
        _logger = logger;
        _problemDetailsFactory = problemDetailsFactory;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            _logger.LogError("{exception}", error);

            var problemDetails = _problemDetailsFactory.CreateProblemDetails(
                context,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An error occurred while processing your request.");

            context.Response.ContentType = MediaTypeNames.Application.ProblemJson;
            context.Response.StatusCode = problemDetails.Status!.Value;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
