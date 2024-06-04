using Budget.Application.Categories.Queries.GetById;
using Mapster;

namespace Budget.Api.Models.Accounts;

public record GetCategoryByIdRequest(int CategoryId);

public class GetCategoryByIdRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(GetCategoryByIdRequest GetCategoryByIdRequest, AuthenticatedUserModel CurrentUser), GetCategoryByIdQuery>()
            .Map(dest => dest, src => src.GetCategoryByIdRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}
