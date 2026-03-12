using AgentFramework.Core;
using AgentFramework.Infrastructure;

namespace AgentFramework.Infrastructure.Agents;

/// <summary>
/// Shared prompt construction for the example agents.
/// </summary>
public abstract class LlmAgentBase : IAgent
{
    private readonly OpenAIClient _openAiClient;

    protected LlmAgentBase(OpenAIClient openAiClient)
    {
        _openAiClient = openAiClient;
    }

    public abstract string Name { get; }

    public abstract string Description { get; }

    protected abstract string SpecialtyPrompt { get; }

    public async Task<AgentResult> ExecuteAsync(AgentContext context)
    {
        var prompt = $"""
            {SpecialtyPrompt}

            User request:
            {context.Input}

            Conversation history:
            {string.Join(Environment.NewLine, context.ConversationHistory)}
            """;

        var output = await _openAiClient.GenerateCompletionAsync(prompt);

        return new AgentResult(
            output,
            new Dictionary<string, object>
            {
                ["agentName"] = Name,
                ["handledAtUtc"] = DateTime.UtcNow,
                ["input"] = context.Input
            },
            [$"{Name} generated a response using the configured LLM client."]);
    }
}
