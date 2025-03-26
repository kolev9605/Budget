using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Authentication;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Budget.Application.Authentication.Commands;

public record RegistrationCommand(
    string Username,
    string Password,
    string Email) : IRequest<ErrorOr<RegistrationResult>>;

public class RegistrationCommandHandler(
    ICategoryRepository _categoryRepository,
    UserManager<ApplicationUser> _userManager)
    : IRequestHandler<RegistrationCommand, ErrorOr<RegistrationResult>>
{
    public async Task<ErrorOr<RegistrationResult>> Handle(RegistrationCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _userManager.FindByNameAsync(request.Username);
        if (userExists is not null)
        {
            return Errors.User.AlreadyExists;
        }

        var initialCategories = await _categoryRepository.GetInitialCategoriesAsync();
        var userCategories = initialCategories
            .Select(c => new UserCategory()
            {
                Category = c,
            }).ToList();

        ApplicationUser user = new()
        {
            Email = request.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = request.Username,
            Categories = userCategories
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return Errors.User.AuthenticationFailed;
        }

        await _userManager.AddToRoleAsync(user, Roles.User);
        return new RegistrationResult(user.Id);
    }
}
