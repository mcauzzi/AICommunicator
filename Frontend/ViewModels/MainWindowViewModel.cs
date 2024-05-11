using System;
using System.Reactive;
using Frontend.Services;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Frontend.ViewModels;

public class MainWindowViewModel : ReactiveObject, IScreen
{
    public IRoutingManager RoutingManager { get; }
    public RoutingState    Router  { get; } = new RoutingState();

    // The command that navigates a user back.
    public ReactiveCommand<Unit, IRoutableViewModel?> GoBack => Router.NavigateBack;

    public ReactiveCommand<Unit, IRoutableViewModel> OpenSettings => GetOpenSettingsCommand();
    
    public MainWindowViewModel(IRoutingManager routingManager)
    {
        RoutingManager = routingManager;
        RoutingManager.RegisterMainScreen(this);
        Router.Navigate.Execute(RoutingManager.GetViewModelFromPath("Chat"));
    }
    private ReactiveCommand<Unit, IRoutableViewModel> GetOpenSettingsCommand()
    {
        return ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(RoutingManager.GetViewModelFromPath("Settings")));
    }
}