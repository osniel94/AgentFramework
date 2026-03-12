namespace AgentFramework.Core;

/// <summary>
/// Normalized agent output including text, structured payloads, and execution logs.
/// </summary>
public sealed class AgentResult
{
    public AgentResult(
        string output,
        Dictionary<string, object>? structuredData = null,
        List<string>? logs = null)
    {
        Output = output;
        StructuredData = structuredData ?? [];
        Logs = logs ?? [];
    }

    public string Output { get; set; }

    public Dictionary<string, object> StructuredData { get; }

    public List<string> Logs { get; }
}
