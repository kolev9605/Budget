using Budget.Domain.Common.Errors;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Categories;
using ErrorOr;
using MediatR;

namespace Budget.Application.Categories.Queries.GetById;

public record GetCategoryByIdQuery(
    Guid CategoryId,
    string UserId
) : IRequest<ErrorOr<CategoryModel>>;

public class GetCategoryByIdQueryHandler(
    ICategoryRepository _categoryRepository
) : IRequestHandler<GetCategoryByIdQuery, ErrorOr<CategoryModel>>
{
    public async Task<ErrorOr<CategoryModel>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository
            .GetByIdWithSubcategoriesMappedAsync(request.CategoryId, request.UserId);

        if (category == null)
        {
            return Errors.Category.NotFound;
        }

        return category;
    }
}
