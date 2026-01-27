using Budget.Api.Interfaces;
using Budget.Common;
using Budget.Domain.Entities;

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
