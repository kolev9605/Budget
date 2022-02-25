using Budget.Core.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Budget.Repositories
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IRecordRepository, RecordRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();

            return services;
        }
    }
}
