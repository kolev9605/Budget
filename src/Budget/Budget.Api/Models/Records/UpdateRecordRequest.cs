using Budget.Application.Records.Commands;
using Budget.Domain.Entities;
using Mapster;

namespace Budget.Api.Models.Records;

public record UpdateRecordRequest(
    Guid Id,
    string Note,
    decimal Amount,
    Guid AccountId,
    Guid CategoryId,
    Guid PaymentTypeId,
    RecordType RecordType,
    DateTimeOffset RecordDate,
    Guid? FromAccountId);

public class UpdateRecordRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(UpdateRecordRequest UpdateRecordRequest, AuthenticatedUserModel CurrentUser), UpdateRecordCommand>()
            .Map(dest => dest, src => src.UpdateRecordRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}
