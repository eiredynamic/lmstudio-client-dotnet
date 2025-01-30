using System.Text;
using System.Text.Json;

namespace Eiredynamic.LMStudio.Client;

public interface IChatService
{
    public abstract IAsyncEnumerable<string> Chat(string usrPrompt, string sysPromt, string endpoint, bool includeReasoning);
}
public class ConcreteChatService : IChatService
{
    private static readonly HashSet<string> _thinkTags = new() { "<think>", "</think>" };

    private static readonly HttpClient _httpClient = new HttpClient()
    {
        Timeout = TimeSpan.FromSeconds(60),
    };
    public async IAsyncEnumerable<string> Chat(string usrPrompt, string sysPromt, string endpoint, bool includeReasoning)
    {
        if (!Uri.IsWellFormedUriString(endpoint, UriKind.Absolute))
        {
            throw new ArgumentException("Malformed URI");
        }

        var requestBody = new
        {
            stream = true,                        // request streaming
            messages = new[]
               {
                    new { role = "system", content = sysPromt },
                    new { role = "user", content = usrPrompt }
               }
        };

        // Serialize the request body
        string jsonPayload = JsonSerializer.Serialize(requestBody);

        // Prepare the request
        var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
        };

        bool skipOutput = false;
        // Send request. Use ResponseHeadersRead to start reading the body as it arrives.

        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        // Get the response stream
        using var responseStream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(responseStream);

        while (!reader.EndOfStream)
        {
            string? line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.StartsWith("data:"))
            {
                string jsonLine = line.Substring("data:".Length).Trim();
                using JsonDocument doc = JsonDocument.Parse(jsonLine);

                // Validate if finished.
                if (doc.RootElement.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0 &&
                    choices[0].TryGetProperty("finish_reason", out var finishReason) && finishReason.GetString() == "stop")
                {
                    yield break;
                }

                string content = "";
                if (choices[0].TryGetProperty("delta", out var delta) &&
                    delta.TryGetProperty("content", out var contentProp))
                {
                    content = contentProp.GetString() ?? "";
                }

                if (!includeReasoning && _thinkTags.Contains(content))
                {
                    skipOutput = content == "<think>";
                    continue;
                }

                if (!skipOutput && !string.IsNullOrEmpty(content))
                {
                    yield return content;

                }
            }
        }
    }
}
public class DummyChatService : IChatService
{
    public async IAsyncEnumerable<string> Chat(string usrPrompt, string sysPromt, string endpoint, bool includeReasoning)
    {
        if (!Uri.IsWellFormedUriString(endpoint, UriKind.Absolute))
        {
            throw new ArgumentException("Malformed URI");
        }
        foreach (string word in usrPrompt.Split(" ", StringSplitOptions.RemoveEmptyEntries))
        {
            yield return word;
            await Task.Delay(100); // Simulate async work
        }
        yield return endpoint;
        yield return includeReasoning.ToString();
    }
}