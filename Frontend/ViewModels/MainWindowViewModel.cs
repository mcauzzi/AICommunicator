using System;
using System.Reactive;
using Frontend.Services;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Frontend.ViewModels;

public class MainWindowViewModel : ReactiveObject, IScreen
{
    public IRoutingManager RoutingManager { get; }
    public RoutingState    Router         { get; } = new RoutingState();

    // The command that navigates a user back.
    public ReactiveCommand<Unit, IRoutableViewModel?> GoBack => Router.NavigateBack;

    public ReactiveCommand<Unit, IRoutableViewModel> OpenSettings => GetOpenSettingsCommand();
    public ReactiveCommand<Unit, IRoutableViewModel> OpenChat     => GetOpenChatCommand();



    public MainWindowViewModel(IRoutingManager routingManager)
    {
        RoutingManager = routingManager;
        RoutingManager.RegisterMainScreen(this);
        Router.Navigate.Execute(RoutingManager.GetViewModelFromPath("Chat")!);
    }

    public MainWindowViewModel()
    {
    }

    private ReactiveCommand<Unit, IRoutableViewModel> GetOpenSettingsCommand()
    {
        return ReactiveCommand.CreateFromObservable(() =>
                                                        Router.Navigate.Execute(RoutingManager
                                                                                        .GetViewModelFromPath("Settings") ??
                                                                                    throw new
                                                                                        Exception("Settings ViewModel not Found")));
    }
    private ReactiveCommand<Unit, IRoutableViewModel> GetOpenChatCommand()
    {
        return ReactiveCommand.CreateFromObservable(() =>
                                                        Router.Navigate.Execute(RoutingManager
                                                                                        .GetViewModelFromPath("Chat") ??
                                                                                    throw new
                                                                                        Exception("Settings ViewModel not Found")));
    }
}