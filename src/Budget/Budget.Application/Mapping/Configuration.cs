using Budget.Application.Models.Accounts;
using Budget.Application.Models.Categories;
using Budget.Application.Models.Records;
using Budget.Domain.Entities;
using Mapster;
using System;
using System.Linq;

namespace Budget.Application.Mapping
{
    public class Configuration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Account, AccountModel>()
                .Map(dest => dest.Balance, src => src.Records.Select(r => r.Amount).Sum() + src.InitialBalance);

            config.NewConfig<Category, CategoryModel>().MaxDepth(2);

            config.NewConfig<Record, RecordsExportModel>()
                .Map(dest => dest.Account, src => src.Account.Name)
                .Map(dest => dest.FromAccount, src => src.FromAccount.Name)
                .Map(dest => dest.PaymentType, src => src.PaymentType.Name)
                .Map(dest => dest.Category, src => src.Category.Name);
        }
    }
}
