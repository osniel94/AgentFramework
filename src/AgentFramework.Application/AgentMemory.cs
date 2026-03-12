using System.Collections.Concurrent;

namespace AgentFramework.Application;

/// <summary>
/// Shared memory store that allows agents in the same workflow to exchange state.
/// </summary>
public sealed class AgentMemory
{
    private readonly ConcurrentDictionary<string, object> _values = new(StringComparer.OrdinalIgnoreCase);

    public IReadOnlyDictionary<string, object> Snapshot() => new Dictionary<string, object>(_values);

    public void Set(string key, object value) => _values[key] = value;

    public bool TryGetValue(string key, out object? value) => _values.TryGetValue(key, out value);

    public T? Get<T>(string key)
    {
        if (!_values.TryGetValue(key, out var value))
        {
            return default;
        }

        return value is T typedValue ? typedValue : default;
    }
}
