using AgentFramework.Core;

namespace AgentFramework.Application;

/// <summary>
/// Chooses the best agent for a request using lightweight routing heuristics.
/// </summary>
public sealed class AgentCoordinator
{
    private readonly AgentRegistry _agentRegistry;

    public AgentCoordinator(AgentRegistry agentRegistry)
    {
        _agentRegistry = agentRegistry;
    }

    public IAgent SelectAgent(string input)
    {
        var normalizedInput = input.Trim().ToLowerInvariant();
        var availableAgents = _agentRegistry.ListAvailableAgents();

        var candidateName =
            normalizedInput.Contains("oracle", StringComparison.Ordinal) || normalizedInput.Contains("sql", StringComparison.Ordinal)
                ? "Oracle SQL Optimizer"
                : normalizedInput.Contains("test", StringComparison.Ordinal) || normalizedInput.Contains("coverage", StringComparison.Ordinal)
                    ? "Test Generation"
                    : normalizedInput.Contains("refactor", StringComparison.Ordinal) || normalizedInput.Contains("clean up", StringComparison.Ordinal)
                        ? "Refactoring"
                        : normalizedInput.Contains(".net", StringComparison.Ordinal) || normalizedInput.Contains("c#", StringComparison.Ordinal) || normalizedInput.Contains("architecture", StringComparison.Ordinal)
                            ? ".NET Architect"
                            : availableAgents.FirstOrDefault()?.Name;

        if (candidateName is null)
        {
            throw new InvalidOperationException("No agents have been registered.");
        }

        return _agentRegistry.Resolve(candidateName)
            ?? throw new InvalidOperationException($"Coordinator selected '{candidateName}', but it was not registered.");
    }
}
