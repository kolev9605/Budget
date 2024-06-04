using Budget.Domain.Common.Errors;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Categories;
using ErrorOr;
using MediatR;

namespace Budget.Application.Categories.Queries.GetById;

public record GetAllCategoriesQuery(
    string UserId
) : IRequest<ErrorOr<IEnumerable<CategoryModel>>>;

public class GetAllCategoriesQueryHandler(
    ICategoryRepository _categoryRepository
) : IRequestHandler<GetAllCategoriesQuery, ErrorOr<IEnumerable<CategoryModel>>>
{
    public async Task<ErrorOr<IEnumerable<CategoryModel>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllWithSubcategoriesCategoryModelsAsync(request.UserId);

        return categories.ToErrorOr();
    }
}
