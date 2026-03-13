using AgentFramework.Core;

namespace AgentFramework.Application;

/// <summary>
/// Central registry for the agent catalog exposed to the console and coordinator.
/// </summary>
public sealed class AgentRegistry
{
    private readonly Dictionary<string, IAgent> _agents = new(StringComparer.OrdinalIgnoreCase);

    public AgentRegistry(IEnumerable<IAgent> agents)
    {
        foreach (var agent in agents)
        {
            Register(agent);
        }
    }

    public void Register(IAgent agent)
    {
        ArgumentNullException.ThrowIfNull(agent);

        if (!_agents.TryAdd(agent.Name, agent))
        {
            throw new InvalidOperationException($"An agent named '{agent.Name}' is already registered.");
        }
    }

    public IAgent? Resolve(string name) => _agents.TryGetValue(name, out var agent) ? agent : null;

    public IReadOnlyCollection<IAgent> ListAvailableAgents() => _agents.Values.OrderBy(agent => agent.Name).ToArray();
}
