using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Api.Domain.Common.Errors;
using Budget.Api.Domain.Entities;
using Budget.Api.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Categories;

public class GetCategoryByIdEndpoint : IEndpoint
{
    public class Request
    {
        public Guid CategoryId { get; set; }
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
        public List<SubCategory> SubCategories { get; set; } = new();

        public class SubCategory
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = null!;
            public CategoryType CategoryType { get; set; }
            public Guid? ParentCategoryId { get; set; }
            public bool IsInitial { get; set; }
            public DateTimeOffset CreatedOn { get; set; }
            public DateTimeOffset UpdatedOn { get; set; }
        }
    }

    public record Query(
        Guid CategoryId,
        string UserId) : IRequest<ErrorOr<Response>>;

    public class QueryHandler : IRequestHandler<Query, ErrorOr<Response>>
    {
        private readonly BudgetDbContext _dbContext;

        public QueryHandler(BudgetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorOr<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories
                .AsNoTracking()
                .Include(c => c.Users)
                .Include(c => c.SubCategories)
                .Where(c => c.Id == query.CategoryId)
                .Where(c => c.Users.Any(uc => uc.UserId == query.UserId))
                .Select(c => new Response
                {
                    Id = c.Id,
                    Name = c.Name,
                    CategoryType = c.CategoryType,
                    ParentCategoryId = c.ParentCategoryId,
                    IsInitial = c.IsInitial,
                    CreatedOn = c.CreatedOn,
                    UpdatedOn = c.UpdatedOn,
                    SubCategories = c.SubCategories.Select(sc => new Response.SubCategory
                    {
                        Id = sc.Id,
                        Name = sc.Name,
                        CategoryType = sc.CategoryType,
                        ParentCategoryId = sc.ParentCategoryId,
                        IsInitial = sc.IsInitial,
                        CreatedOn = sc.CreatedOn,
                        UpdatedOn = sc.UpdatedOn
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (category is null)
            {
                return Errors.Category.NotFound;
            }

            return category;
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
