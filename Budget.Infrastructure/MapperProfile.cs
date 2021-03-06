namespace Budget.Infrastructure
{
    using AutoMapper;
    using Budget.Data.Models;
    using Budget.Services.Models;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            this.CreateMap<Transaction, TransactionServiceModel>();
            this.CreateMap<Category, CategoryInfoServiceModel>();
        }
    }
}
