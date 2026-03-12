namespace AgentFramework.Core;

/// <summary>
/// Represents a specialized software-engineering agent that can handle a user request.
/// </summary>
public interface IAgent
{
    string Name { get; }

    string Description { get; }

    Task<AgentResult> ExecuteAsync(AgentContext context);
}
