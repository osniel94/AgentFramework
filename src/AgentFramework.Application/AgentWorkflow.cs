using AgentFramework.Core;

namespace AgentFramework.Application;

/// <summary>
/// Executes a sequence of agents while persisting outputs into shared memory.
/// </summary>
public sealed class AgentWorkflow
{
    private readonly AgentMemory _agentMemory;

    public AgentWorkflow(AgentMemory agentMemory)
    {
        _agentMemory = agentMemory;
    }

    public async Task<AgentResult> ExecuteSequentialAsync(IEnumerable<IAgent> agents, AgentContext context)
    {
        var workflowLogs = new List<string>();
        AgentResult? lastResult = null;

        foreach (var agent in agents)
        {
            workflowLogs.Add($"Starting agent '{agent.Name}'.");
            context.SharedMemory["lastSelectedAgent"] = agent.Name;

            lastResult = await agent.ExecuteAsync(context);

            _agentMemory.Set(agent.Name, lastResult.Output);
            context.SharedMemory[agent.Name] = lastResult.Output;
            context.SharedMemory["lastAgentOutput"] = lastResult.Output;
            context.ConversationHistory.Add($"{agent.Name}: {lastResult.Output}");

            workflowLogs.AddRange(lastResult.Logs);
            workflowLogs.Add($"Completed agent '{agent.Name}'.");
        }

        return new AgentResult(
            lastResult?.Output ?? "No agents were executed.",
            lastResult?.StructuredData is null ? [] : new Dictionary<string, object>(lastResult.StructuredData),
            workflowLogs);
    }
}
