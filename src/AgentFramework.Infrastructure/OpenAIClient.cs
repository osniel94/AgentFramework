using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace AgentFramework.Infrastructure;

/// <summary>
/// Minimal LLM client abstraction built on HttpClient and compatible with OpenAI-style chat completion APIs.
/// </summary>
public sealed class OpenAIClient
{
    private readonly HttpClient _httpClient;
    private readonly OpenAIClientOptions _options;

    public OpenAIClient(HttpClient httpClient, OpenAIClientOptions options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public async Task<string> GenerateCompletionAsync(string prompt)
    {
        if (string.IsNullOrWhiteSpace(_options.Endpoint))
        {
            return $"[{_options.ProviderName}] Offline completion placeholder: {prompt[..Math.Min(prompt.Length, 400)]}";
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, BuildRequestUri());
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (!string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);
        }

        request.Content = JsonContent.Create(new
        {
            model = _options.Model,
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        });

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        await using var responseStream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(responseStream);

        if (document.RootElement.TryGetProperty("choices", out var choices) &&
            choices.GetArrayLength() > 0 &&
            choices[0].TryGetProperty("message", out var message) &&
            message.TryGetProperty("content", out var content))
        {
            return content.GetString() ?? string.Empty;
        }

        return document.RootElement.GetRawText();
    }

    private string BuildRequestUri()
    {
        var endpoint = _options.Endpoint.TrimEnd('/');
        var path = _options.CompletionPath.StartsWith('/') ? _options.CompletionPath : $"/{_options.CompletionPath}";
        return $"{endpoint}{path}";
    }
}
