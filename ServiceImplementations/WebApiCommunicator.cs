using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ChatDtos;
using ServiceInterfaces;

namespace ServiceImplementations;

public class WebApiCommunicator : IWebApiCommunicator
{
    public WebApiCommunicator(string baseAddress, string webApiKey)
    {
        WebApiKey = webApiKey;
        ChatUrl   = "";
        Client    = new HttpClient() { BaseAddress = new Uri(baseAddress) };
    }
    public async Task<ChatResponse?> SendChatRequest(string input)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, ChatUrl);
        req.Headers.Add("Authorization", $"Bearer {WebApiKey}");
        req.Content = JsonContent.Create(new ChatRequest() { message = input });
        var response = await Client.SendAsync(req);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ChatResponse>();
    }

    private string     ChatUrl   { get; }
    private string     WebApiKey { get; }
    private HttpClient Client    { get; }
}