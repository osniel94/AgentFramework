namespace AgentFramework.Core;

/// <summary>
/// Carries the current request, conversation, and shared state across agents.
/// </summary>
public sealed class AgentContext
{
    public AgentContext(
        string input,
        Dictionary<string, object>? metadata = null,
        Dictionary<string, object>? sharedMemory = null,
        List<string>? conversationHistory = null)
    {
        Input = input;
        Metadata = metadata ?? [];
        SharedMemory = sharedMemory ?? [];
        ConversationHistory = conversationHistory ?? [];
    }

    public string Input { get; set; }

    public Dictionary<string, object> Metadata { get; }

    public Dictionary<string, object> SharedMemory { get; }

    public List<string> ConversationHistory { get; }
}
