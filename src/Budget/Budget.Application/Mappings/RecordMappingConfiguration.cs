using Budget.Domain.Entities;
using Budget.Domain.Models.Records;
using Mapster;
using System;

namespace Budget.Application.Mappings;

public class RecordMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Record, RecordsExportModel>()
            .Map(dest => dest.Account, src => src.Account.Name)
            .Map(dest => dest.FromAccount, src => src.FromAccount.Name)
            .Map(dest => dest.PaymentType, src => src.PaymentType.Name)
            .Map(dest => dest.Category, src => src.Category.Name);
    }
}
