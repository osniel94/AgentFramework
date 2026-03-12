using AgentFramework.Infrastructure;

namespace AgentFramework.Infrastructure.Agents;

public sealed class RefactoringAgent : LlmAgentBase
{
    public RefactoringAgent(OpenAIClient openAiClient)
        : base(openAiClient)
    {
    }

    public override string Name => "Refactoring";

    public override string Description => "Improves code structure, readability, and maintainability without changing behavior.";

    protected override string SpecialtyPrompt =>
        "You are a code refactoring assistant. Focus on reducing complexity, clarifying intent, and preserving behavior.";
}
