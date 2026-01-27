using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Categories;

public class CreateCategoryEndpoint : IEndpoint
{
    public class Request
    {
        public string Name { get; set; } = null!;
        public CategoryType CategoryType { get; set; }
        public Guid? ParentCategoryId { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
    }

    public record Command(
        string Name,
        CategoryType CategoryType,
        Guid? ParentCategoryId,
        string UserId) : IRequest<ErrorOr<Response>>;

    public class CommandHandler : IRequestHandler<Command, ErrorOr<Response>>
    {
        private readonly BudgetDbContext _dbContext;

        public CommandHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorOr<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var existingCategory = await _dbContext.Categories
                .Include(c => c.Users)
                .Where(c => c.Name == command.Name)
                .FirstOrDefaultAsync(cancellationToken);

            // If the existing category matches with the one passed in the command
            if (existingCategory != null &&
                existingCategory.CategoryType == command.CategoryType &&
                existingCategory.ParentCategoryId == command.ParentCategoryId)
            {
                if (existingCategory.Users.Any(uc => uc.UserId == command.UserId))
                {
                    return Errors.Category.AlreadyExistsForUser;
                }

                existingCategory.Users.Add(new UserCategory
                {
                    UserId = command.UserId
                });

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new Response { Id = existingCategory.Id };
            }
            else
            {
                if (existingCategory != null)
                {
                    return Errors.Category.AlreadyExists;
                }

                var category = new Category
                {
                    Name = command.Name,
                    CategoryType = command.CategoryType,
                    ParentCategoryId = command.ParentCategoryId,
                    CreatedOn = DateTimeOffset.UtcNow,
                    UpdatedOn = DateTimeOffset.UtcNow,
                    Users = new List<UserCategory>
                    {
                        new UserCategory { UserId = command.UserId }
                    }
                };

                _dbContext.Categories.Add(category);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new Response { Id = category.Id };
            }
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapPost("/categories", async (
                Request request,
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var command = new Command(
                    request.Name,
                    request.CategoryType,
                    request.ParentCategoryId,
                    currentUser.Id);
                var result = await mediator.Send(command);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("Categories");
    }
}
