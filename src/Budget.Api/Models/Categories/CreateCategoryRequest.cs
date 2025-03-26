using Budget.Application.Categories.Commands.Create;
using Budget.Domain.Entities;
using Mapster;

namespace Budget.Api.Models.Categories;

public record CreateCategoryRequest(
    string Name,
    CategoryType CategoryType,
    Guid? ParentCategoryId);

public class CreateCategoryRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateCategoryRequest CreateCategoryRequest, AuthenticatedUserModel CurrentUser), CreateCategoryCommand>()
            .Map(dest => dest, src => src.CreateCategoryRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}
