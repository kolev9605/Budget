using Budget.Application.Records.Queries;
using Budget.Domain.Entities;
using Mapster;

namespace Budget.Api.Models.Records;

public record GetAllRecordsRequest(
    Guid? AccountId,
    RecordType? RecordType,
    Guid? CategoryId,
    DateTime? StartDateRange,
    DateTime? EndDateRange,
    int PageNumber,
    int? PageSize);

public class GetAllRecordsRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(GetAllRecordsRequest GetAllRecordsRequest, AuthenticatedUserModel CurrentUser), GetAllRecordsQuery>()
            .Map(dest => dest, src => src.GetAllRecordsRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}
