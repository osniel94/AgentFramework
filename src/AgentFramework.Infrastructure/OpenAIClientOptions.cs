namespace AgentFramework.Infrastructure;

/// <summary>
/// Keeps the LLM client provider-agnostic so the HTTP transport can target OpenAI-compatible APIs or alternates.
/// </summary>
public sealed class OpenAIClientOptions
{
    public string ProviderName { get; set; } = "MockProvider";

    public string Endpoint { get; set; } = string.Empty;

    public string CompletionPath { get; set; } = "/v1/chat/completions";

    public string Model { get; set; } = "gpt-4.1-mini";

    public string ApiKey { get; set; } = string.Empty;
}
