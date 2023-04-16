using Budget.Application.Models.Accounts;
using Budget.Application.Models.Records;
using Budget.Domain.Entities;
using Mapster;
using System;

namespace Budget.Application.Mapping;

public class RecordMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Record, RecordsExportModel>()
            .Map(dest => dest.Account, src => src.Account.Name)
            .Map(dest => dest.FromAccount, src => src.FromAccount.Name)
            .Map(dest => dest.PaymentType, src => src.PaymentType.Name)
            .Map(dest => dest.Category, src => src.Category.Name);

        config.NewConfig<(CreateRecordModel Record, DateTime DateCreated, decimal Amount), Record>()
            .Map(dest => dest, src => src.Record)
            .Map(dest => dest.DateCreated, src => src.DateCreated)
            .Map(dest => dest.Amount, src => src.Amount);
    }
}