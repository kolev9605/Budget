using Budget.Application.Categories.Commands.Delete;
using Mapster;

namespace Budget.Api.Models.Categories;

public record DeleteCategoryRequest(Guid Id);

public class DeleteCategoryRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(DeleteCategoryRequest DeleteCategoryRequest, AuthenticatedUserModel CurrentUser), DeleteCategoryCommand>()
            .Map(dest => dest, src => src.DeleteCategoryRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}
