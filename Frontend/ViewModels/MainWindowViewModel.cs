using System;
using System.Reactive;
using ReactiveUI;

namespace Frontend.ViewModels;

public class MainWindowViewModel : ReactiveObject, IScreen
{
    public RoutingState Router { get; } = new RoutingState();

    // The command that navigates a user back.
    public ReactiveCommand<Unit, IRoutableViewModel?> GoBack => Router.NavigateBack;

    public MainWindowViewModel()
    {
        ChatViewModel = new ChatControlViewModel(this);
        // Manage the routing state. Use the Router.Navigate.Execute
        // command to navigate to different view models. 
        //
        // Note, that the Navigate.Execute method accepts an instance 
        // of a view model, this allows you to pass parameters to 
        // your view models, or to reuse existing view models.
        //
        Router.Navigate.Execute(ChatViewModel);
    }

    public ChatControlViewModel ChatViewModel { get; set; }
}