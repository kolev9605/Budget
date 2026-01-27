using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Domain.Common.Errors;
using Budget.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Categories;

public class DeleteCategoryEndpoint : IEndpoint
{
    public class Request
    {
        public Guid CategoryId { get; set; }
    }

    public class Response
    {
    }

    public record Command(
        Guid CategoryId,
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
                .Include(c => c.Records)
                .Include(c => c.SubCategories)
                .ThenInclude(sc => sc.Records)
                .Where(c => c.Id == command.CategoryId)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingCategory is null)
            {
                return Errors.Category.NotFound;
            }

            var userCategory = existingCategory.Users.FirstOrDefault(uc => uc.UserId == command.UserId);
            if (userCategory is null)
            {
                return Errors.Category.NotFound;
            }

            // Check if category has records
            if (existingCategory.Records.Any() || existingCategory.SubCategories.Any(sc => sc.Records.Any()))
            {
                return Errors.Category.HasRecords;
            }

            // Check if category has sub-categories
            if (existingCategory.SubCategories.Any())
            {
                return Errors.Category.HasSubCategories;
            }

            // If only one user, delete the category entirely
            if (existingCategory.Users.Count == 1 && existingCategory.Users.First().UserId == command.UserId)
            {
                _dbContext.Categories.Remove(existingCategory);
            }
            else
            {
                // Otherwise, just remove the user from the category
                existingCategory.Users.Remove(userCategory);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new Response();
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapDelete("/categories/{categoryId}", async (
                [AsParameters] Request request,
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var command = new Command(request.CategoryId, currentUser.Id);
                var result = await mediator.Send(command);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("Categories");
    }
}
