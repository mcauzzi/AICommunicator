using System;
using System.Collections.Generic;
using DynamicData.Binding;
using Frontend.Globals;
using InternalDtos;
using ReactiveUI;
using ServiceInterfaces;

namespace Frontend.ViewModels;

public class SettingsViewModel : ReactiveObject, IRoutableViewModel
{
    public string?                 UrlPathSegment     => "Settings";
    public IScreen                 HostScreen         { get; } = null!;
    public HashSet<AudioInterface> OutputAudioDevices { get; set; }

    public HashSet<AudioInterface> InputAudioDevices { get; set; }

    public SettingsViewModel(IScreen screen, IAudioInterfaceManager audioInterfaceManager)
    {
        InputAudioDevices     = audioInterfaceManager.InputDevices;
        OutputAudioDevices    = audioInterfaceManager.OutputDevices;
        HostScreen            = screen;
        AudioInterfaceManager = audioInterfaceManager;
        this.WhenPropertyChanged(x => x.SelectedInputAudioSource)
            .Subscribe(x => GlobalAppState.SelectedInputAudioDevice = x.Value);
        this.WhenPropertyChanged(x => x.SelectedOutputAudioSource)
            .Subscribe(x => GlobalAppState.SelectedOutputAudioDevice = x.Value);
    }

    public SettingsViewModel()
    {
        InputAudioDevices  = new();
        OutputAudioDevices = new();
    }

    public AudioInterface? SelectedOutputAudioSource
    {
        get => _selectedOutputAudioSource;
        set => this.RaiseAndSetIfChanged(ref _selectedOutputAudioSource, value);
    }

    public AudioInterface? SelectedInputAudioSource
    {
        get => _selectedInputAudioSource;
        set => this.RaiseAndSetIfChanged(ref _selectedInputAudioSource, value);
    }

    public void Refresh()
    {
        InputAudioDevices  = AudioInterfaceManager.InputDevices;
        OutputAudioDevices = AudioInterfaceManager.OutputDevices;
    }

    private AudioInterface?         _selectedOutputAudioSource;
    private AudioInterface?         _selectedInputAudioSource;
    private IAudioInterfaceManager AudioInterfaceManager { get; }
}