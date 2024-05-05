using System.Collections.Generic;
using Frontend.Models;
using ReactiveUI;

namespace Frontend.ViewModels;

public class ChatControlViewModel:ReactiveObject, IRoutableViewModel
{
    public List<ChatMessage> Messages { get; } = new()
                                                 {
                                                     new() { Message = "AAA", Sender = "SENDER1" },
                                                     new() { Message = "AAA", Sender = "SENDER2" },
                                                     new() { Message = "AAA", Sender = "SENDER1" },
                                                     new() { Message = "AAA", Sender = "SENDER2" },
                                                     new() { Message = "AAA", Sender = "SENDER1" },
                                                     new() { Message = "AAA", Sender = "SENDER2" },
                                                 };

    public string CurrentMessage { get; } = "";

    public ChatControlViewModel()
    {
        HostScreen = null;
    }
    public ChatControlViewModel(IScreen screen) => HostScreen = screen;
    public string? UrlPathSegment => "Chat";
    public IScreen HostScreen     { get; }
}