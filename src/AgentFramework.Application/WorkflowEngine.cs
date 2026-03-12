using AgentFramework.Core;

namespace AgentFramework.Application;

/// <summary>
/// Orchestrates single-agent and multi-agent execution and appends execution tracing.
/// </summary>
public sealed class WorkflowEngine
{
    private readonly AgentCoordinator _agentCoordinator;
    private readonly AgentWorkflow _agentWorkflow;
    private readonly AgentMemory _agentMemory;

    public WorkflowEngine(
        AgentCoordinator agentCoordinator,
        AgentWorkflow agentWorkflow,
        AgentMemory agentMemory)
    {
        _agentCoordinator = agentCoordinator;
        _agentWorkflow = agentWorkflow;
        _agentMemory = agentMemory;
    }

    public async Task<AgentResult> RunAsync(AgentContext context)
    {
        var agent = _agentCoordinator.SelectAgent(context.Input);
        var result = await _agentWorkflow.ExecuteSequentialAsync([agent], context);
        result.Logs.Insert(0, $"Workflow engine selected '{agent.Name}'.");
        return result;
    }

    public async Task<AgentResult> RunWorkflowAsync(IEnumerable<IAgent> agents, AgentContext context)
    {
        var result = await _agentWorkflow.ExecuteSequentialAsync(agents, context);
        result.Logs.Insert(0, "Workflow engine executed a multi-agent workflow.");
        result.StructuredData["sharedMemorySnapshot"] = _agentMemory.Snapshot();
        return result;
    }
}
