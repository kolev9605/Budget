using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces;
using Budget.Domain.Models.Authentication;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Budget.Api.Endpoints.Authentication;

public class LoginEndpoint : IEndpoint
{
    public class Request
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class Response
    {
        public string Token { get; set; } = null!;
        public DateTimeOffset ValidTo { get; set; }
    }

    public record Query(
        string Email,
        string Password) : IRequest<ErrorOr<Response>>;

    public class QueryHandler : IRequestHandler<Query, ErrorOr<Response>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly UserManager<ApplicationUser> _userManager;

        public QueryHandler(IJwtTokenGenerator jwtTokenGenerator, UserManager<ApplicationUser> userManager)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userManager = userManager;
        }

        public async Task<ErrorOr<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(query.Email);
            if (user is null)
            {
                return Errors.User.NotFound;
            }

            if (!await _userManager.CheckPasswordAsync(user, query.Password))
            {
                return Errors.User.AuthenticationFailed;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var jwtTokenResult = _jwtTokenGenerator.GenerateToken(userRoles, user.Id, user.Email!);

            return new Response { Token = jwtTokenResult.Token, ValidTo = jwtTokenResult.ValidTo };
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapPost("/authentication/login", async (
                Request request,
                IMediator mediator) =>
            {
                var query = new Query(request.Email, request.Password);
                var result = await mediator.Send(query);

                return result.MatchResponse();
            })
            .WithTags("Authentication");
    }
}
