using AgentFramework.Infrastructure;

namespace AgentFramework.Infrastructure.Agents;

public sealed class TestGenerationAgent : LlmAgentBase
{
    public TestGenerationAgent(OpenAIClient openAiClient)
        : base(openAiClient)
    {
    }

    public override string Name => "Test Generation";

    public override string Description => "Produces test strategies and example unit or integration tests for software changes.";

    protected override string SpecialtyPrompt =>
        "You are a software test generation assistant. Produce high-value tests, edge cases, and verification strategy for the described change.";
}
