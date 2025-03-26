using Budget.Application.Records.Commands;
using Budget.Domain.Entities;
using Mapster;

namespace Budget.Api.Models.Records;

public record CreateRecordRequest(
    string Note,
    decimal Amount,
    Guid AccountId,
    Guid CategoryId,
    Guid PaymentTypeId,
    RecordType RecordType,
    DateTimeOffset RecordDate,
    Guid? FromAccountId);

public class CreateRecordRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateRecordRequest CreateRecordRequest, AuthenticatedUserModel CurrentUser), CreateRecordCommand>()
            .Map(dest => dest, src => src.CreateRecordRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}
