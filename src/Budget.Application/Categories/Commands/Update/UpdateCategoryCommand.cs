using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Categories;
using ErrorOr;
using Mapster;
using MediatR;

namespace Budget.Application.Categories.Commands.Update;

public record UpdateCategoryCommand(
    Guid Id,
    string Name,
    CategoryType CategoryType,
    Guid? ParentCategoryId,
    string UserId
) : IRequest<ErrorOr<CategoryModel>>;

public class CreateAccountCommandHandler(
    ICategoryRepository _categoryRepository)
    : IRequestHandler<UpdateCategoryCommand, ErrorOr<CategoryModel>>
{
    public async Task<ErrorOr<CategoryModel>> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var existingCategory = await _categoryRepository.GetByIdWithSubcategoriesAsync(command.Id, command.UserId);
        if (existingCategory == null)
        {
            return Errors.Category.NotFound;
        }

        // Made sub-category
        if (command.ParentCategoryId.HasValue)
        {
            if (existingCategory.SubCategories.Any())
            {
                return Errors.Category.CannotBecomeSubcategory;
            }
        }

        // TODO: Mapster
        existingCategory.ParentCategoryId = command.ParentCategoryId;
        existingCategory.CategoryType = command.CategoryType;
        existingCategory.Name = command.Name;

        var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);

        return updatedCategory.Adapt<CategoryModel>();
    }
}
