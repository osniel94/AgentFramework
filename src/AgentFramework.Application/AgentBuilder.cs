using AgentFramework.Core;

namespace AgentFramework.Application;

/// <summary>
/// Builds agent instances from registered factories and applies optional configuration hooks.
/// </summary>
public sealed class AgentBuilder
{
    private readonly Dictionary<string, Func<IAgent>> _factories = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, Action<IAgent>> _configurations = new(StringComparer.OrdinalIgnoreCase);

    public AgentBuilder RegisterFactory(string agentName, Func<IAgent> factory)
    {
        _factories[agentName] = factory;
        return this;
    }

    public AgentBuilder Configure(string agentName, Action<IAgent> configure)
    {
        _configurations[agentName] = configure;
        return this;
    }

    public IAgent Build(string agentName)
    {
        if (!_factories.TryGetValue(agentName, out var factory))
        {
            throw new InvalidOperationException($"No factory has been registered for agent '{agentName}'.");
        }

        var agent = factory();

        if (_configurations.TryGetValue(agentName, out var configure))
        {
            configure(agent);
        }

        return agent;
    }
}
