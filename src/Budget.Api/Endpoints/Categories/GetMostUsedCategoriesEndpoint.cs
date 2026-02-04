using Budget.Api.Helpers;
using Budget.Api.Interfaces;
using Budget.Api.Domain.Constants;
using Budget.Api.Domain.Interfaces.Services;
using Budget.Api.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budget.Api.Endpoints.Categories;

public class GetMostUsedCategoriesEndpoint : IEndpoint
{
    public class Request
    {
        public int? Count { get; set; } = 10;
    }

    public class Response
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int Count { get; set; }
    }

    public record Query(
        string UserId,
        int Count) : IRequest<ErrorOr<IEnumerable<Response>>>;

    public class QueryHandler : IRequestHandler<Query, ErrorOr<IEnumerable<Response>>>
    {
        private readonly BudgetDbContext _dbContext;
        private readonly ICacheManager _cacheManager;

        public QueryHandler(BudgetDbContext dbContext, ICacheManager cacheManager)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        public async Task<ErrorOr<IEnumerable<Response>>> Handle(Query query, CancellationToken cancellationToken)
        {
            return await _cacheManager.GetOrCreateAsync(
                CacheConstants.MostUsedCategories.Key,
                CacheConstants.MostUsedCategories.ExpirationInSeconds,
                async () =>
                {
                    var mostUsedCategories = await _dbContext.Records
                        .AsNoTracking()
                        .Include(r => r.Account)
                        .Include(r => r.Category)
                        .Where(r => r.Account.UserId == query.UserId)
                        .GroupBy(r => r.Category)
                        .OrderByDescending(g => g.Count())
                        .Take(query.Count)
                        .Select(g => new Response
                        {
                            Id = g.Key.Id,
                            Name = g.Key.Name,
                            Count = g.Count()
                        })
                        .ToListAsync(cancellationToken);

                    return mostUsedCategories.AsEnumerable().ToErrorOr();
                });
        }
    }

    public static void Map(WebApplication app)
    {
        app
            .MapGet("/categories/most-used", async (
                [AsParameters] Request request,
                IMediator mediator,
                HttpContext httpContext) =>
            {
                var currentUser = httpContext.GetCurrentUser();
                var query = new Query(currentUser.Id, request.Count ?? 10);
                var result = await mediator.Send(query);

                return result.MatchResponse();
            })
            .RequireAuthorization()
            .WithTags("Categories");
    }
}
