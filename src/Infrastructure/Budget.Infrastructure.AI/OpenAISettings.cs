namespace Budget.Infrastructure.AI;

public record OpenAISettings
{
    public const string SectionName = "OpenAI";

    public string Endpoint { get; init; } = string.Empty;

    public string Key { get; init; } = string.Empty;

    public string Model { get; init; } = string.Empty;
}
