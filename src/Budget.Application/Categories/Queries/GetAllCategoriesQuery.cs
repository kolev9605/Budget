using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Categories;
using ErrorOr;
using MediatR;

namespace Budget.Application.Categories.Queries.GetById;

public record GetAllCategoriesQuery(
    bool PrimaryOnly,
    string UserId
) : IRequest<ErrorOr<IEnumerable<CategoryModel>>>;

public class GetAllCategoriesQueryHandler(
    ICategoryRepository _categoryRepository
) : IRequestHandler<GetAllCategoriesQuery, ErrorOr<IEnumerable<CategoryModel>>>
{
    public async Task<ErrorOr<IEnumerable<CategoryModel>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        if (request.PrimaryOnly)
        {
            var primaryCategories = await _categoryRepository.GetAllPrimaryAsync(request.UserId);
            return primaryCategories.ToErrorOr();
        }
        else
        {
            var allCategories = await _categoryRepository.GetAllAsync(request.UserId);
            return allCategories.ToErrorOr();
        }
    }
}
