using Budget.Application.Categories.Queries.GetById;
using Mapster;

namespace Budget.Api.Models.Categories;

public record GetAllCategoriesRequest(
    bool PrimaryOnly = false
);

public class GetAllCategoriesRequestMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(GetAllCategoriesRequest GetAllCategoriesRequest, AuthenticatedUserModel CurrentUser), GetAllCategoriesQuery>()
            .Map(dest => dest, src => src.GetAllCategoriesRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}
