using Budget.Application.Records.Commands;
using Mapster;

namespace Budget.Api.Models.Records;

public record DeleteRecordRequest(Guid Id);

public class DeleteRecordRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(DeleteRecordRequest DeleteRecordRequest, AuthenticatedUserModel CurrentUser), DeleteRecordCommand>()
            .Map(dest => dest, src => src.DeleteRecordRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}
