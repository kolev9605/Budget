using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Domain.Common.Errors;
using Budget.Domain.Models.Categories;
using Budget.Infrastructure.Persistence;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Categories;

public class GetCategoryByIdEndpoint : IEndpoint
{
    public class Request
    {
        public Guid CategoryId { get; set; }
    }

    public record Query(
        Guid CategoryId,
        string UserId) : IRequest<ErrorOr<CategoryModel>>;

    public class QueryHandler : IRequestHandler<Query, ErrorOr<CategoryModel>>
    {
        private readonly BudgetDbContext _dbContext;

        public QueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorOr<CategoryModel>> Handle(Query query, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories
                .AsNoTracking()
                .Include(c => c.Users)
                .Include(c => c.SubCategories)
                .Where(c => c.Id == query.CategoryId)
                .Where(c => c.Users.Any(uc => uc.UserId == query.UserId))
                .FirstOrDefaultAsync(cancellationToken);

            if (category is null)
            {
                return Errors.Category.NotFound;
            }

            return category.Adapt<CategoryModel>();
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapGet("/categories/{categoryId}", async (
                [AsParameters] Request request,
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var query = new Query(request.CategoryId, currentUser.Id);
                var result = await mediator.Send(query);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("Categories");
    }
}
