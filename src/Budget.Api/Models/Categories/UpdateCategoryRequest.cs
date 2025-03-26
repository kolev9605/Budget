using Budget.Application.Categories.Commands.Update;
using Budget.Domain.Entities;
using Mapster;

namespace Budget.Api.Models.Categories;

public record UpdateCategoryRequest(
    Guid Id,
    string Name,
    CategoryType CategoryType,
    Guid? ParentCategoryId);

public class UpdateCategoryRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(UpdateCategoryRequest UpdateCategoryRequest, AuthenticatedUserModel CurrentUser), UpdateCategoryCommand>()
            .Map(dest => dest, src => src.UpdateCategoryRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}
