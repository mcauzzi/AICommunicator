using System;
using Frontend.ViewModels;
using Frontend.Views;
using ReactiveUI;

namespace Frontend;

public class AppViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T? viewModel, string? contract = null) => viewModel switch
                                                                             {
                                                                                 ChatControlViewModel context =>
                                                                                     new ChatControlView()
                                                                                     { DataContext = context },
                                                                                 SettingsViewModel context =>
                                                                                     new SettingsView()
                                                                                     { DataContext = context },
                                                                                 _ => throw new
                                                                                     ArgumentOutOfRangeException(nameof
                                                                                         (viewModel))
                                                                             };
}