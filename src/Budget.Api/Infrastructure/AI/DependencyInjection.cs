using System.ClientModel;
using Budget.Api.Domain.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using OpenAI;

namespace Budget.Api.AI;

public static class DependencyInjection
{
    public static IServiceCollection AddAI(this IServiceCollection services, IConfiguration configuration)
    {
        var openAISettings = new OpenAISettings();
        configuration.Bind(OpenAISettings.SectionName, openAISettings);
        services.AddSingleton(Options.Create(openAISettings));

        // Create a client to our GitHub Model
        var client = new OpenAIClient(new ApiKeyCredential(openAISettings.Key), new OpenAIClientOptions
        {
            Endpoint = new Uri(openAISettings.Endpoint),
        });

        // Create a chat completion service
        var builder = Kernel.CreateBuilder();
        builder.AddOpenAIChatCompletion(openAISettings.Model, client);

        // Get the chat completion service
        Kernel kernel = builder.Build();

        services.AddSingleton(kernel);
        services.AddSingleton<IQuickAddService, QuickAddService>();

        return services;
    }
}
