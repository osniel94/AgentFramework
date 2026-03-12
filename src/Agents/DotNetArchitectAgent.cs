using AgentFramework.Infrastructure;

namespace AgentFramework.Infrastructure.Agents;

public sealed class DotNetArchitectAgent : LlmAgentBase
{
    public DotNetArchitectAgent(OpenAIClient openAiClient)
        : base(openAiClient)
    {
    }

    public override string Name => ".NET Architect";

    public override string Description => "Designs .NET application architecture, layering, and service boundaries.";

    protected override string SpecialtyPrompt =>
        "You are a senior .NET software architect. Provide implementation guidance with emphasis on clean architecture, maintainability, and pragmatic delivery.";
}
