using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceInterfaces;

namespace ServiceImplementations;

public static class AddServices
{
    public static IServiceCollection AddAiCommunicationServices(this IServiceCollection services)
    {
        services.AddSingleton<ISpeech, AzureSpeech>();
        services.AddSingleton<ILLMWebApiCommunicator, AnythingLLMCommunicator>();
        services.AddSingleton<IAudioInterfacePicker, ConsoleAudioInterfacePicker>();
        services.AddSingleton<IAudioInterfaceManager,AudioInterfaceManager>();
        return services;
    }
}