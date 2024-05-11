using System.Collections.Generic;
using InternalDtos;
using ReactiveUI;
using ServiceInterfaces;

namespace Frontend.ViewModels;

public class SettingsViewModel : ReactiveObject, IRoutableViewModel
{
    public  string?                UrlPathSegment        => "Settings";
    public  IScreen                HostScreen            { get; } = null!;
    private IAudioInterfaceManager AudioInterfaceManager { get; }

    public SettingsViewModel(IScreen screen, IAudioInterfaceManager audioInterfaceManager)
    {
        InputAudioDevices     = audioInterfaceManager.InputDevices;
        OutputAudioDevices    = audioInterfaceManager.OutputDevices;
        HostScreen            = screen;
        AudioInterfaceManager = audioInterfaceManager;
    }

    public SettingsViewModel()
    {
        InputAudioDevices  = new();
        OutputAudioDevices = new();
    }


    public HashSet<AudioInterface> OutputAudioDevices { get; set; }

    public HashSet<AudioInterface> InputAudioDevices { get; set; }

    public void Refresh()
    {
        InputAudioDevices  = AudioInterfaceManager.InputDevices;
        OutputAudioDevices = AudioInterfaceManager.OutputDevices;
    }
}