using System.Net.Http.Json;

namespace AnythingLLMComunicator;

public class WebApiCommunicator
{
    public WebApiCommunicator(string baseAddress, string webApiKey)
    {
        WebApiKey = webApiKey;
        Client    = new HttpClient() { BaseAddress = new Uri(baseAddress) };
    }


    public async Task<ChatResponse?> SendChatRequest(string chatUrl, string input)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, chatUrl);
        req.Headers.Add("Authorization", $"Bearer {WebApiKey}");
        req.Content = JsonContent.Create(new ChatRequest() { message = input });
        var response = await Client.SendAsync(req);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ChatResponse>();
    }

    private string     WebApiKey { get; }
    private HttpClient Client    { get; }
}