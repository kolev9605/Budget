using Budget.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Budget.Persistance
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BudgetDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("BudgetPosgres")));

            services.AddScoped<IBudgetDbContext>(provider => provider.GetService<BudgetDbContext>());

            //services.AddDbContext<BudgetDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("BudgetSqlServer")));

            return services;
        }
    }
}
