using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Api.Domain.Common.Errors;
using Budget.Api.Domain.Entities;
using Budget.Api.Domain.Models.Authentication;
using Budget.Api.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Authentication;

public class RegisterEndpoint : IEndpoint
{
    public class Request
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class Response
    {
        public string UserId { get; set; } = null!;
    }

    public record Command(
        string Email,
        string Password) : IRequest<ErrorOr<Response>>;

    public class CommandHandler : IRequestHandler<Command, ErrorOr<Response>>
    {
        private readonly BudgetDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommandHandler(BudgetDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<ErrorOr<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var userExists = await _userManager.FindByNameAsync(command.Email);
            if (userExists is not null)
            {
                return Errors.User.AlreadyExists;
            }

            var initialCategories = await _dbContext.Categories
                .Where(c => c.IsInitial)
                .ToListAsync(cancellationToken);

            var userCategories = initialCategories
                .Select(c => new UserCategory { CategoryId = c.Id })
                .ToList();

            var user = new ApplicationUser
            {
                Email = command.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = command.Email,
                Categories = userCategories
            };

            var result = await _userManager.CreateAsync(user, command.Password);
            if (!result.Succeeded)
            {
                return Errors.User.AuthenticationFailed;
            }

            await _userManager.AddToRoleAsync(user, Roles.User);

            return new Response { UserId = user.Id };
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapPost("/authentication/register", async (
                Request request,
                IMediator mediator) =>
            {
                var command = new Command(request.Email, request.Password);
                var result = await mediator.Send(command);

                return result.MatchResponse();
            })
            .WithTags("Authentication");
    }
}
