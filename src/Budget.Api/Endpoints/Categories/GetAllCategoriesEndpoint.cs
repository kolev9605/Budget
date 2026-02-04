using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Api.Domain.Entities;
using Budget.Api.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Categories;

public class GetAllCategoriesEndpoint : IEndpoint
{
    public class Request
    {
        public bool? PrimaryOnly { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public CategoryType CategoryType { get; set; }

        public Guid? ParentCategoryId { get; set; }

        public bool IsInitial { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }
    }

    public record Query(
        bool PrimaryOnly,
        string UserId) : IRequest<ErrorOr<IEnumerable<Response>>>;

    public class QueryHandler : IRequestHandler<Query, ErrorOr<IEnumerable<Response>>>
    {
        private readonly BudgetDbContext _dbContext;

        public QueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorOr<IEnumerable<Response>>> Handle(Query query, CancellationToken cancellationToken)
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
                .Select(c => new Response
                {
                    Id = c.Id,
                    Name = c.Name,
                    CategoryType = c.CategoryType,
                    ParentCategoryId = c.ParentCategoryId,
                    IsInitial = c.IsInitial,
                    CreatedOn = c.CreatedOn,
                    UpdatedOn = c.UpdatedOn
                })
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
