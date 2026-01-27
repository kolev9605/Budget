using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Domain.Models.Categories;
using Budget.Infrastructure.Persistence;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Categories;

public class GetAllCategoriesEndpoint : IEndpoint
{
    public class Request
    {
        public bool? PrimaryOnly { get; set; }
    }

    public record Query(
        bool PrimaryOnly,
        string UserId) : IRequest<ErrorOr<IEnumerable<CategoryModel>>>;

    public class QueryHandler : IRequestHandler<Query, ErrorOr<IEnumerable<CategoryModel>>>
    {
        private readonly BudgetDbContext _dbContext;

        public QueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorOr<IEnumerable<CategoryModel>>> Handle(Query query, CancellationToken cancellationToken)
        {
            var categoriesQuery = _dbContext.Categories
                .AsNoTracking()
                .Include(c => c.Users)
                .Where(c => c.Users.Any(uc => uc.UserId == query.UserId));

            if (query.PrimaryOnly == true)
            {
                categoriesQuery = categoriesQuery.Where(c => c.ParentCategoryId == null);
            }

            var categories = await categoriesQuery
                .OrderBy(c => c.Name)
                .ProjectToType<CategoryModel>()
                .ToListAsync(cancellationToken);

            return categories.AsEnumerable().ToErrorOr();
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapGet("/categories", async (
                [AsParameters] Request request,
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var primaryOnly = request.PrimaryOnly ?? false;
                var query = new Query(primaryOnly, currentUser.Id);
                var result = await mediator.Send(query);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("Categories");
    }
}
