using Budget.Application.Categories.Queries.GetById;
using Mapster;

namespace Budget.Api.Models.Categories;

public record GetAllSubcategoriesRequest(Guid ParentCategoryId);

public class GetAllSubcategoriesRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(GetAllSubcategoriesRequest GetAllSubcategoriesRequest, AuthenticatedUserModel CurrentUser), GetAllSubcategoriesQuery>()
            .Map(dest => dest, src => src.GetAllSubcategoriesRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}

