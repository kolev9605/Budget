using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Categories;
using ErrorOr;
using MediatR;

namespace Budget.Application.Categories.Queries.GetById;

public record GetAllSubcategoriesQuery(
    Guid ParentCategoryId,
    string UserId
) : IRequest<ErrorOr<IEnumerable<CategoryModel>>>;

public class GetAllSubcategoriesQueryHandler(
    ICategoryRepository _categoryRepository
) : IRequestHandler<GetAllSubcategoriesQuery, ErrorOr<IEnumerable<CategoryModel>>>
{
    public async Task<ErrorOr<IEnumerable<CategoryModel>>> Handle(GetAllSubcategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetSubcategoriesByParentCategoryIdMappedAsync(request.ParentCategoryId, request.UserId);

        return categories.ToErrorOr();
    }
}
