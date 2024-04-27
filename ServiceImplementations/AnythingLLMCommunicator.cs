using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ChatDtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceImplementations.Configs;
using ServiceInterfaces;

namespace ServiceImplementations;

// ReSharper disable once InconsistentNaming
public class AnythingLLMCommunicator : ILLMWebApiCommunicator
{
    public AnythingLLMCommunicator(IOptions<AnythingLLMConfig> config,ILogger<AnythingLLMCommunicator> logger)
    {
        WebApiKey   = config.Value.WebApiKey;
        Logger = logger;
        ChatUrl     =$"api/v1/workspace/{config.Value.WorkspaceSlug}/chat";
        Client      = new HttpClient() { BaseAddress = new Uri(config.Value.BaseAddress) };
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
    private  ILogger<AnythingLLMCommunicator> Logger    { get; }
    private HttpClient Client    { get; }
}