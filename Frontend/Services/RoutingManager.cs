using System;
using Frontend.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
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
                                                                     "Settings" => new SettingsViewModel(MainScreen,
                                                                          ServiceProvider
                                                                              .GetRequiredService<
                                                                                  IAudioInterfaceManager>()),
                                                                     "Chat" => new ChatControlViewModel(MainScreen),
                                                                     _      => null
                                                                 };

    private IServiceProvider ServiceProvider { get; }
}