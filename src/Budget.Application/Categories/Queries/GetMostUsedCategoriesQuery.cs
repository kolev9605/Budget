using Budget.Domain.Constants;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Categories;
using ErrorOr;
using MediatR;

namespace Budget.Application.Categories.Queries;

public record GetMostUsedCategoriesQuery(
    string UserId,
    int Count) : IRequest<ErrorOr<IEnumerable<GetMostUsedCategoriesResult>>>;

public class GetMostUsedCategoriesQueryHandler : IRequestHandler<GetMostUsedCategoriesQuery, ErrorOr<IEnumerable<GetMostUsedCategoriesResult>>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICacheManager _cacheManager;

    public GetMostUsedCategoriesQueryHandler(ICategoryRepository categoryRepository, ICacheManager cacheManager)
    {
        _categoryRepository = categoryRepository;
        _cacheManager = cacheManager;
    }

    public async Task<ErrorOr<IEnumerable<GetMostUsedCategoriesResult>>> Handle(GetMostUsedCategoriesQuery query, CancellationToken cancellationToken)
    {
        return await _cacheManager.GetOrCreateAsync(
            CacheConstants.MostUsedCategories.Key,
            CacheConstants.MostUsedCategories.ExpirationInSeconds,
            async () =>
            {
                var categories = await _categoryRepository.GetMostUsedCategoriesAsync(query.UserId, query.Count);
                return categories.ToErrorOr();
            });
    }
}
