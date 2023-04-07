using Budget.Application.Interfaces;
using Budget.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Budget.Persistance
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BudgetDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("BudgetPosgres")));

            services.AddScoped<IBudgetDbContext>(provider => provider.GetService<BudgetDbContext>());

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<BudgetDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
