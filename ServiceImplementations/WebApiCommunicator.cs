using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ChatDtos;
using Microsoft.Extensions.Logging;
using ServiceInterfaces;

namespace ServiceImplementations;

public class WebApiCommunicator : IWebApiCommunicator
{
    public WebApiCommunicator(string baseAddress, string webApiKey,ILogger<WebApiCommunicator> logger)
    {
        WebApiKey   = webApiKey;
        Logger = logger;
        ChatUrl     = "";
        Client      = new HttpClient() { BaseAddress = new Uri(baseAddress) };
    }
    public async Task<ChatResponse?> SendChatRequest(string input)
    {
        using var scope=Logger.BeginScope(new Dictionary<string, string> { [nameof(ChatUrl)]=ChatUrl,[nameof(Client.BaseAddress)]=Client.BaseAddress.ToString() });
        Logger.LogInformation("Sending new chat {ChatMessage}", input);
        var req = new HttpRequestMessage(HttpMethod.Post, ChatUrl);
        req.Headers.Add("Authorization", $"Bearer {WebApiKey}");
        req.Content = JsonContent.Create(new ChatRequest() { Message = input });
        var response = await Client.SendAsync(req);
        response.EnsureSuccessStatusCode();
        
        var serializedResponse = await response.Content.ReadFromJsonAsync<ChatResponse>();
        Logger.LogInformation("Message successfully sent! Response:{ChatResponse}",serializedResponse);
        return serializedResponse;
    }

    private string ChatUrl   { get; }
    private string WebApiKey { get; }
    private  ILogger<WebApiCommunicator> Logger    { get; }
    private HttpClient Client    { get; }
}