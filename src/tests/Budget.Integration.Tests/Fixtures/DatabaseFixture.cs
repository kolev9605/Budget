using Budget.Api.Domain.Models.Authentication;
using Budget.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Xunit;

namespace Budget.Integration.Tests.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    private PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:17")
            .WithDatabase("budget_test")
            .WithUsername("testuser")
            .WithPassword("testpassword")
            .Build();
    private IServiceProvider? _serviceProvider;
    private BudgetDbContext? _dbContext;

    public T GetRequiredService<T>() where T : notnull
    {
        if (_serviceProvider is null)
        {
            throw new InvalidOperationException("Service provider not initialized");
        }

        return _serviceProvider.GetRequiredService<T>();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        // var seedScriptPath = Path.Combine(AppContext.BaseDirectory, "scripts", "initial-seeder.sql");
        // await _container.ExecScriptAsync(seedScriptPath);

        var services = new ServiceCollection();

        DependencyInjection.AddPersistence(services, _container.GetConnectionString());

        // In the app, AddIdentityCore(...).AddDefaultTokenProviders() is wired under ASP.NET Core hosting,
        // and Data Protection is auto-registered by the web host (through the default service setup).
        // In the test fixture you’re manually building a ServiceCollection,
        // so you don’t get those host defaults unless you add them yourself.
        services.AddDataProtection();

        _serviceProvider = services.BuildServiceProvider();

        _dbContext = _serviceProvider.GetRequiredService<BudgetDbContext>();

        // Apply migrations
        await _dbContext.Database.MigrateAsync();

        var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        if (!await roleManager.RoleExistsAsync(Roles.User))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.User));
        }
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
        // if (_dbContext != null)
        // {
        //     await _dbContext.Database.EnsureDeletedAsync();
        //     await _dbContext.DisposeAsync();
        // }

        // if (_serviceProvider is IAsyncDisposable disposable)
        // {
        //     await disposable.DisposeAsync();
        // }

        // if (_container != null)
        // {
        //     await _container.StopAsync();
        //     await _container.DisposeAsync();
        // }
    }
}
