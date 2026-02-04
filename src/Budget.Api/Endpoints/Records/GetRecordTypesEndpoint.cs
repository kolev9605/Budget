using Budget.Api.Interfaces;
using Budget.Api.Helpers;
using Budget.Api.Domain.Entities;

namespace Budget.Api.Endpoints.Records;

public class GetRecordTypesEndpoint : IEndpoint
{
    public static void Map(WebApplication app)
    {
        app
            .MapGet("/records/types", () =>
            {
                var recordTypes = EnumHelpers.GetListFromEnum<RecordType>();
                return Results.Ok(recordTypes);
            })
            .RequireAuthorization()
            .WithTags("Records");
    }
}
