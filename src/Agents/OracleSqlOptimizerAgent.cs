using AgentFramework.Infrastructure;

namespace AgentFramework.Infrastructure.Agents;

public sealed class OracleSqlOptimizerAgent : LlmAgentBase
{
    public OracleSqlOptimizerAgent(OpenAIClient openAiClient)
        : base(openAiClient)
    {
    }

    public override string Name => "Oracle SQL Optimizer";

    public override string Description => "Analyzes Oracle SQL and recommends performance, indexing, and query-shape improvements.";

    protected override string SpecialtyPrompt =>
        "You are an Oracle SQL performance specialist. Suggest tuning improvements, likely bottlenecks, and safer rewrites with concise reasoning.";
}
