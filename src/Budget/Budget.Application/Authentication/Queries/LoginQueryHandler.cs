using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Authentication;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Budget.Application.Authentication.Queries;

public record LoginQuery(
    string Username,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;

public class LoginQueryHandler(
    IJwtTokenGenerator _jwtTokenGenerator,
    UserManager<ApplicationUser> _userManager)
    : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            return Errors.User.UserNotFound;
        }

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Errors.User.AuthenticationFailed;
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        var jwtTokenResult = _jwtTokenGenerator.GenerateToken(userRoles, user.Id, user.Email!);

        var tokenModel = new AuthenticationResult(jwtTokenResult.Token, jwtTokenResult.ValidTo);

        return tokenModel;

    }
}
