namespace Budget.Web.Infrastructure
{
    using AutoMapper;
    using Budget.Data.Models;
    using Budget.Data.Models.Enums;
    using Budget.Services.Models;
    using Budget.Web.Areas.User.ViewModels;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            this.MapServiceModels();
            this.MapViewModels();
        }

        private void MapServiceModels()
        {
            this.CreateMap<Transaction, TransactionServiceModel>();
            this.CreateMap<Category, CategoryServiceModel>();
            this.CreateMap<Category, UserCategoryServiceModel>();
        }

        private void MapViewModels()
        {
            this.CreateMap<IEnumerable<UserCategoryServiceModel>, CategoriesViewModel>()
                .ForMember(
                    c => c.Categories,
                    cfg => cfg.MapFrom(c => c.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    })));

            this.CreateMap<IEnumerable<TransactionType>, AddCategoryViewModel>()
                .ForMember(c => c.TransactionTypes,
                    cfg => cfg.MapFrom(c => c.Select(x => new SelectListItem
                    {
                        Text = Enum.GetName(typeof(TransactionType), x),
                        Value = ((int)x).ToString()
                    })));



            this.CreateMap<IEnumerable<TransactionServiceModel>, TransactionsViewModel>()
                .ForMember(c => c.Balance, cfg => cfg.MapFrom(
                    cc => cc.FirstOrDefault().User.Balance))
                .ForMember(c => c.HasTransaction, cfg => cfg.MapFrom(
                    cc => cc.Any()));

            this.CreateMap<TransactionServiceModel, TransactionDataViewModel>();                

            this.CreateMap<IEnumerable<TransactionServiceModel>, ChartViewModel>()
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
                    .Select(c => c.Category.RgbColorValue))))
                .ForMember(c => c.IsCurrentChartEmpty, cfg => cfg.MapFrom(c => c.Any()));
        }
    }
}
