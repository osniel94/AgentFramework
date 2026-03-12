using AgentFramework.Application;
using AgentFramework.Core;
using AgentFramework.Infrastructure;
using AgentFramework.Infrastructure.Agents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// The console composes the framework here: application services, shared infrastructure, and concrete agents.
builder.Services.AddSingleton(new OpenAIClientOptions
{
    ProviderName = "ConfigurableOpenAIClient",
    Endpoint = Environment.GetEnvironmentVariable("OPENAI_ENDPOINT") ?? string.Empty,
    ApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? string.Empty,
    Model = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4.1-mini"
});

builder.Services.AddHttpClient<OpenAIClient>();
builder.Services.AddSingleton<AgentMemory>();
builder.Services.AddSingleton<AgentRegistry>();
builder.Services.AddSingleton<AgentBuilder>();
builder.Services.AddSingleton<AgentCoordinator>();
builder.Services.AddSingleton<AgentWorkflow>();
builder.Services.AddSingleton<WorkflowEngine>();

builder.Services.AddSingleton<IAgent, DotNetArchitectAgent>();
builder.Services.AddSingleton<IAgent, OracleSqlOptimizerAgent>();
builder.Services.AddSingleton<IAgent, RefactoringAgent>();
builder.Services.AddSingleton<IAgent, TestGenerationAgent>();

using var host = builder.Build();

var registry = host.Services.GetRequiredService<AgentRegistry>();
var coordinator = host.Services.GetRequiredService<AgentCoordinator>();
var workflowEngine = host.Services.GetRequiredService<WorkflowEngine>();

Console.WriteLine("Available agents:");
foreach (var agent in registry.ListAvailableAgents())
{
    Console.WriteLine($"- {agent.Name}: {agent.Description}");
}

Console.WriteLine();
Console.Write("Enter a software engineering request (blank to exit): ");
var input = Console.ReadLine();

if (string.IsNullOrWhiteSpace(input))
{
    return;
}

var selectedAgent = coordinator.SelectAgent(input);
var context = new AgentContext(input);
var result = await workflowEngine.RunAsync(context);

Console.WriteLine();
Console.WriteLine($"Selected agent: {selectedAgent.Name}");
Console.WriteLine("Output:");
Console.WriteLine(result.Output);
Console.WriteLine();
Console.WriteLine("Logs:");
foreach (var log in result.Logs)
{
    Console.WriteLine($"- {log}");
}
