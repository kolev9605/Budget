using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Api.Domain.Common.Errors;
using Budget.Api.Domain.Entities;
using Budget.Api.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Categories;

public class UpdateCategoryEndpoint : IEndpoint
{
    public class Request
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public CategoryType CategoryType { get; set; }
        public Guid? ParentCategoryId { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
    }

    public record Command(
        Guid CategoryId,
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
                .Include(c => c.SubCategories)
                .Where(c => c.Id == command.CategoryId)
                .Where(c => c.Users.Any(uc => uc.UserId == command.UserId))
                .FirstOrDefaultAsync(cancellationToken);

            if (existingCategory is null)
            {
                return Errors.Category.NotFound;
            }

            // Cannot become sub-category if has sub-categories
            if (command.ParentCategoryId.HasValue)
            {
                if (existingCategory.SubCategories.Any())
                {
                    return Errors.Category.CannotBecomeSubcategory;
                }
            }

            existingCategory.ParentCategoryId = command.ParentCategoryId;
            existingCategory.CategoryType = command.CategoryType;
            existingCategory.Name = command.Name;
            existingCategory.UpdatedOn = DateTimeOffset.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new Response { Id = existingCategory.Id };
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapPut("/categories", async (
                Request request,
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var command = new Command(
                    request.Id,
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
