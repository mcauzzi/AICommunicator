using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Frontend.Globals;
using Models;
using ReactiveUI;
using ServiceInterfaces;

namespace Frontend.ViewModels;

public class ChatControlViewModel : ReactiveObject, IRoutableViewModel
{
    private string _currentMessage = "";

    public ChatControlViewModel()
    {
        HostScreen = null;
        Messages   = new();
    }

    public ChatControlViewModel(IScreen screen, ISpeech speechService, ILLMWebApiCommunicator communicator)
    {
        HostScreen    = screen;
        SpeechService = speechService;
        Communicator  = communicator;
        Messages      = new();
        RegisterCommand =
            ReactiveCommand.CreateFromTask(async x =>
                                           {
                                               if (GlobalAppState.SelectedInputAudioDevice is null)
                                               {
                                                   return;
                                               }
                                               var inputMessage = await SpeechService.VoiceToText(GlobalAppState
                                                                      .SelectedInputAudioDevice);
                                               CurrentMessage = inputMessage;
                                           });
        SubmitCommand = ReactiveCommand.CreateFromTask(async x =>
                                                       {
                                                           Messages.Add(new ChatMessage()
                                                                        { Message = CurrentMessage, Sender = "YOU" });
                                                           var chatResponse =
                                                               await Communicator.SendChatRequest(CurrentMessage);
                                                           if (chatResponse is null)
                                                           {
                                                               return;
                                                           }
                                                           Messages.Add(new ChatMessage()
                                                                        { Message = chatResponse.TextResponse, Sender = "AI" });
                                                           if (GlobalAppState.SelectedOutputAudioDevice is null)
                                                           {
                                                               return;
                                                           }
                                                           await SpeechService.TextToAudio(chatResponse.TextResponse,
                                                            GlobalAppState.SelectedOutputAudioDevice);
                                                       });
    }

    public ObservableCollection<ChatMessage> Messages       { get; }

    public string CurrentMessage
    {
        get => _currentMessage;
        set => this.RaiseAndSetIfChanged(ref _currentMessage,value);
    }

    public  string?                     UrlPathSegment  => "Chat";
    public  IScreen                     HostScreen      { get; }
    public  ReactiveCommand<Unit, Unit> RegisterCommand { get; }
    public  ReactiveCommand<Unit, Unit> SubmitCommand   { get; }
    private ISpeech                     SpeechService   { get; }
    private ILLMWebApiCommunicator      Communicator    { get; }
}