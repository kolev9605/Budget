using Budget.Application.Accounts.Queries.GetAll;
using Budget.Application.Categories.Queries.GetById;
using Budget.Application.Records.Queries;
using Mapster;

namespace Budget.Api.Models;

public record AuthenticatedUserModel(
    string Id,
    string Email);

public class AuthenticatedUserModelMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AuthenticatedUserModel, GetAllAccountsQuery>()
            .Map(dest => dest.UserId, src => src.Id);

        config.NewConfig<AuthenticatedUserModel, GetAllCategoriesQuery>()
            .Map(dest => dest.UserId, src => src.Id);

        config.NewConfig<AuthenticatedUserModel, GetAllPrimaryQuery>()
            .Map(dest => dest.UserId, src => src.Id);

        config.NewConfig<AuthenticatedUserModel, GetRecordsDateRangeQuery>()
            .Map(dest => dest.UserId, src => src.Id);
    }
}
