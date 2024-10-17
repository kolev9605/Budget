using Budget.Application.Accounts.Queries.GetById;
using Budget.Application.Services;
using Budget.Domain.Interfaces.Services;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Budget.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddServices();
            services.AddMappings();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetAccountByIdQueryHandler>());

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICacheManager, CacheManager>();
            services.AddScoped<IRecordService, RecordService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IPaymentTypeService, PaymentTypeService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            // services.AddScoped<IUserService, UserService>();

            return services;
        }

        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}
