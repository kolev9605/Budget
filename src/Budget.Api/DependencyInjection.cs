using System.Reflection;
using System.Text.Json.Serialization;
using Budget.Api.Endpoints.Accounts;
using Budget.Api.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http.Features;

namespace Budget.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddMappings();

        services.AddMemoryCache();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddControllers().AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.Configure<FormOptions>(options =>
        {
            options.ValueLengthLimit = int.MaxValue;
            options.MultipartBodyLengthLimit = int.MaxValue;
            options.MemoryBufferThreshold = int.MaxValue;
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddHealthChecks();

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

    public static void MapEndpoints(this WebApplication app)
    {
        var endpointTypes = typeof(Program).Assembly
            .GetTypes()
            .Where(t =>
                t is { IsAbstract: false, IsInterface: false } &&
                typeof(IEndpoint).IsAssignableFrom(t));

        foreach (var type in endpointTypes)
        {
            var method = type.GetMethod(
                nameof(IEndpoint.Map),
                BindingFlags.Public | BindingFlags.Static);

            method!.Invoke(null, new object[] { app });
        }
    }
}
