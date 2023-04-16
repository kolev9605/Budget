﻿using Budget.Application.Interfaces.Services;
using Budget.Application.Mapping;
using Budget.Application.Services;
using Budget.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Budget.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddServices();
            services.AddMappings();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICacheManager, CacheManager>();
            services.AddScoped<IPaginationManager, PaginationManager>();
            services.AddScoped<IRecordService, RecordService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IPaymentTypeService, PaymentTypeService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IChartService, ChartService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
