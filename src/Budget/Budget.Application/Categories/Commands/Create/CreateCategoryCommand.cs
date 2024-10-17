using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Categories;
using ErrorOr;
using Mapster;
using MediatR;

namespace Budget.Application.Categories.Commands.Create;

public record CreateCategoryCommand(
    string Name,
    CategoryType CategoryType,
    Guid? ParentCategoryId,
    string UserId
) : IRequest<ErrorOr<CategoryModel>>;

public class CreateAccountCommandHandler(
    ICategoryRepository _categoryRepository)
    : IRequestHandler<CreateCategoryCommand, ErrorOr<CategoryModel>>
{
    public async Task<ErrorOr<CategoryModel>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var existingCategory = await _categoryRepository.GetByNameWithUsersAsync(request.Name);

        // If the existing category matches with the one passed in the createCategoryModel, instead of creating a new category, we should add the user to the UserCategories
        // TODO: Extension method 'ExistsGlobally()'
        if (existingCategory != null &&
            existingCategory.CategoryType == request.CategoryType &&
            existingCategory.ParentCategoryId == request.ParentCategoryId)
        {
            if (existingCategory.Users.Any(uc => uc.UserId == request.UserId))
            {
                return Errors.Category.AlreadyExistsForUser;
            }

            existingCategory.Users.Add(new UserCategory
            {
                UserId = request.UserId
            });

            var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);

            return updatedCategory.Adapt<CategoryModel>();
        }
        else
        {
            if (existingCategory != null)
            {
                return Errors.Category.AlreadyExists;
            }

            var category = request.Adapt<Category>();
            category.Users.Add(new UserCategory
            {
                UserId = request.UserId
            });

            var createdCategory = await _categoryRepository.CreateAsync(category);

            return createdCategory.Adapt<CategoryModel>();
        }
    }
}
