using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Frontend.ViewModels;
using ReactiveUI;

namespace Frontend.Views;

public partial class ChatControlView : ReactiveUserControl<ChatControlViewModel>
{
    public ChatControlView()
    {
        InitializeComponent();
        this.WhenActivated(disposables => { });
    }
}