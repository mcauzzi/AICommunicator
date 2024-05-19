using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ChatDtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Configs;
using ServiceInterfaces;

namespace ServiceImplementations;

// ReSharper disable once InconsistentNaming
public class AnythingLLMCommunicator : ILLMWebApiCommunicator
{
    public AnythingLLMCommunicator(IOptionsSnapshot<AiCommunicatorConfig> config,ILogger<AnythingLLMCommunicator> logger)
    {
        Config    = config;
        Logger    = logger;
    }

    private IOptionsSnapshot<AiCommunicatorConfig> Config { get; set; }

    public async Task<ChatResponse?> SendChatRequest(string input)
    {
        using var scope=Logger.BeginScope(new Dictionary<string, string> { [nameof(ChatUrl)]=ChatUrl,[nameof(Client.BaseAddress)]=Client.BaseAddress.ToString() });
        Logger.LogInformation("Sending new chat {ChatMessage}", input);
        var req = new HttpRequestMessage(HttpMethod.Post, ChatUrl);
        req.Headers.Add("Authorization", $"Bearer {WebApiKey}");
        req.Content = JsonContent.Create(new ChatRequest() { Message = input });
      
        try
        {
            var response = await Client.SendAsync(req);
            var serializedResponse = await response.Content.ReadFromJsonAsync<ChatResponse>();
            Logger.LogInformation("Chat Response received! Response:{ChatResponse}",serializedResponse);
            return serializedResponse;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex,"Error while Sending chat Request");
            return null;
        }
    }
    
    private string                           ChatUrl   =>$"api/v1/workspace/{Config.Value.WorkspaceSlug}/chat";
    private string                           WebApiKey => Config.Value.WebApiKey;
    private ILogger<AnythingLLMCommunicator> Logger    { get; }
    private HttpClient?                      _currentClient;

    private HttpClient Client
    {
        get
        {
            if (_currentClient is null || Config.Value.BaseAddress != _currentClient.BaseAddress.ToString())
            {
                _currentClient = new HttpClient() { BaseAddress = new Uri(Config.Value.BaseAddress) };
            }

            return _currentClient;
        }
    }
}