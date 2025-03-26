using Budget.Domain.Common.Errors;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Categories;
using ErrorOr;
using Mapster;
using MediatR;

namespace Budget.Application.Categories.Commands.Delete;

public record DeleteCategoryCommand(
    Guid Id,
    string UserId) : IRequest<ErrorOr<CategoryModel>>;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ErrorOr<CategoryModel>>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ErrorOr<CategoryModel>> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var existingCategory = await _categoryRepository.GetForDeletionAsync(command.Id, command.UserId);

        if (existingCategory == null)
        {
            return Errors.Category.NotFound;
        }

        if (existingCategory.Records.Any() || existingCategory.SubCategories.Any(sc => sc.Records.Any()))
        {
            return Errors.Category.HasRecords;
        }

        if (existingCategory.SubCategories.Any())
        {
            return Errors.Category.HasSubCategories;
        }

        if (existingCategory.Users.Count == 1 && existingCategory.Users.First().UserId == command.UserId)
        {
            var deletedCategory = await _categoryRepository.DeleteAsync(existingCategory);

            return deletedCategory.Adapt<CategoryModel>();
        }
        else
        {
            var userItem = existingCategory.Users.First(uc => uc.UserId == command.UserId);
            existingCategory.Users.Remove(userItem);

            var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);

            return updatedCategory.Adapt<CategoryModel>();
        }
    }
}
