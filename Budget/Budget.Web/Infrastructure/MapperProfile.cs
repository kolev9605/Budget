namespace Budget.Web.Infrastructure
{
    using AutoMapper;
    using Budget.Data.Models;
    using Budget.Data.Models.Enums;
    using Budget.Services.Models;
    using Budget.Web.Areas.User.ViewModels;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            this.CreateMap<Transaction, TransactionServiceModel>();

            this.CreateMap<Category, CategoryServiceModel>();

            this.CreateMap<IEnumerable<CategoryServiceModel>, AddTransactionViewModel>()
                .ForMember(
                    c => c.Categories,
                    cfg => cfg.MapFrom(c => c.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    })));

            this.CreateMap<IEnumerable<TransactionServiceModel>, ChartViewModel>()
                .ForMember(c => c.OpositeType, cfg => cfg.MapFrom(
                    cc => cc.FirstOrDefault().Category.TransactionType == TransactionType.Expense ? TransactionType.Income : TransactionType.Expense))
                .ForMember(c => c.DataJson, cfg => cfg.MapFrom(cc => JsonConvert.SerializeObject(cc
                    .GroupBy(t => t.Category.Id)
                    .Select(t => new TransactionServiceModel
                    {
                        Id = t.First().Id,
                        Category = t.First().Category,
                        Date = t.First().Date,
                        User = t.First().User,
                        Amount = t.Sum(tt => tt.Amount)
                    })
                    .Select(c => c.Amount))))
                .ForMember(c => c.LabelsJson, cfg => cfg.MapFrom(cc => JsonConvert.SerializeObject(cc
                    .GroupBy(t => t.Category.Id)
                    .Select(t => new TransactionServiceModel
                    {
                        Id = t.First().Id,
                        Category = t.First().Category,
                        Date = t.First().Date,
                        User = t.First().User,
                        Amount = t.Sum(tt => tt.Amount)
                    })
                    .Select(c => c.Category.Name))))
                .ForMember(c => c.ColorsJson, cfg => cfg.MapFrom(cc => JsonConvert.SerializeObject(cc
                    .GroupBy(t => t.Category.Id)
                    .Select(t => new TransactionServiceModel
                    {
                        Id = t.First().Id,
                        Category = t.First().Category,
                        Date = t.First().Date,
                        User = t.First().User,
                        Amount = t.Sum(tt => tt.Amount)
                    })
                    .Select(c => c.Category.RgbColorValue))));

        }
    }
}
