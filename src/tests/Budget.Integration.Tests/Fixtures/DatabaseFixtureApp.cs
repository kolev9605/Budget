using Budget.Api.Domain.Models.Authentication;
using Budget.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Xunit;

namespace Budget.Integration.Tests.Fixtures;

public class DatabaseFixtureApp : WebApplicationFactory<Program>, IAsyncLifetime
{
    private PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:17")
            .WithDatabase("budget_test")
            .WithUsername("testuser")
            .WithPassword("testpassword")
            .Build();
    private IServiceScope? _scope;

    public T GetRequiredService<T>() where T : notnull
    {
        if (_scope is null)
        {
            throw new InvalidOperationException("Service scope not initialized");
        }

        return _scope.ServiceProvider.GetRequiredService<T>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // services.RemoveAll<DbContextOptions<BudgetDbContext>>();
            services.AddPersistence(_container.GetConnectionString());
        });
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        _scope = Services.CreateScope();

        // Apply migrations and seed roles
        var dbContext = _scope.ServiceProvider.GetRequiredService<BudgetDbContext>();

        await dbContext.Database.MigrateAsync();

        var roleManager = _scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        if (!await roleManager.RoleExistsAsync(Roles.User))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.User));
        }
    }

    public new async Task DisposeAsync()
    {
        await _container.StopAsync();
        // if (_scope != null)
        // {
        //     var dbContext = _scope.ServiceProvider.GetRequiredService<BudgetDbContext>();
        //     await dbContext.Database.EnsureDeletedAsync();
        //     _scope.Dispose();
        // }

        // if (_container != null)
        // {
        //     await _container.StopAsync();
        //     await _container.DisposeAsync();
        // }

        await base.DisposeAsync();
    }
}
