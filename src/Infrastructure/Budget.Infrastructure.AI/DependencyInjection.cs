using Budget.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
namespace Budget.Infrastructure.AI;

public static class DependencyInjection
{
    public static IServiceCollection AddAI(this IServiceCollection services, IConfiguration configuration)
    {
        var kernelBuilder = Kernel.CreateBuilder();

        kernelBuilder.AddOpenAIChatCompletion(
            modelId: "llama3.2:3b", // or gpt-3.5-turbo
            apiKey: "<your-openai-api-key>", // Use secure configuration in production
            endpoint: new Uri("http://localhost:11434/v1")
        );

        var kernel = kernelBuilder.Build();
        services.AddSingleton(kernel);
        services.AddSingleton<IQuickAddService, QuickAddService>();


        return services;
    }

}
