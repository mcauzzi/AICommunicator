using System;
using Frontend.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Models;
using ReactiveUI;
using ServiceImplementations.Configs;
using ServiceInterfaces;

namespace Frontend.Services;

public class RoutingManager : IRoutingManager
{
    public RoutingManager(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public void RegisterMainScreen(IScreen mainScreen)
    {
        MainScreen = mainScreen;
    }

    public IScreen MainScreen { get; set; }

    public IRoutableViewModel? GetViewModelFromPath(string s) => s switch
                                                                 {
                                                                     "Settings" => GetSettingsViewModel(),
                                                                     "Chat" => new ChatControlViewModel(MainScreen,
                                                                          ServiceProvider
                                                                              .GetRequiredService<ISpeech>(),
                                                                          ServiceProvider
                                                                              .GetRequiredService<
                                                                                  ILLMWebApiCommunicator>()),
                                                                     _ => null
                                                                 };

    private SettingsViewModel GetSettingsViewModel()
    {
        return new SettingsViewModel(MainScreen, ServiceProvider.GetRequiredService<IAudioInterfaceManager>(),
                                     ServiceProvider.GetRequiredService<ISettingsRepository<CommConfig>>(),
                                     ServiceProvider.GetRequiredService<IOptions<AiCommunicatorConfig>>());
    }

    private IServiceProvider ServiceProvider { get; }
}