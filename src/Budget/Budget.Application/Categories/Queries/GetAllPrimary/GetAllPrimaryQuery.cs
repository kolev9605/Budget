using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Categories;
using ErrorOr;
using MediatR;

namespace Budget.Application.Categories.Queries.GetById;

public record GetAllPrimaryQuery(
    string UserId
) : IRequest<ErrorOr<IEnumerable<CategoryModel>>>;

public class GetAllPrimaryQueryHandler(
    ICategoryRepository _categoryRepository
) : IRequestHandler<GetAllPrimaryQuery, ErrorOr<IEnumerable<CategoryModel>>>
{
    public async Task<ErrorOr<IEnumerable<CategoryModel>>> Handle(GetAllPrimaryQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllPrimaryCategoryModelsAsync(request.UserId);

        return categories.ToErrorOr();
    }
}
