using System;
using System.Collections.Generic;
using System.Reactive;
using DynamicData.Binding;
using Frontend.Globals;
using Microsoft.Extensions.Options;
using Models;
using Models.Configs;
using ReactiveUI;
using ServiceInterfaces;

namespace Frontend.ViewModels;

public class SettingsViewModel : ReactiveObject, IRoutableViewModel,IDisposable
{
    public string?                 UrlPathSegment     => "Settings";
    public IScreen                 HostScreen         { get; } = null!;
    public HashSet<AudioInterface> OutputAudioDevices { get; set; }

    public HashSet<AudioInterface> InputAudioDevices { get; set; }

    public FrontendConfig FrontendConfig
    {
        get => _frontendConfig;
        set => this.RaiseAndSetIfChanged(ref _frontendConfig, value);
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
    public ReactiveCommand<Unit, Unit> UpdateSettingsCommand { get; set; }

    public SettingsViewModel(IScreen screen, IAudioInterfaceManager audioInterfaceManager,
                             ISettingsRepository<FrontendConfig> repo,
                             IOptions<AiCommunicatorConfig> aiCommConfig)
    {
        InputAudioDevices     = audioInterfaceManager.InputDevices;
        OutputAudioDevices    = audioInterfaceManager.OutputDevices;
        HostScreen            = screen;
        AudioInterfaceManager = audioInterfaceManager;
        FrontendConfig = new FrontendConfig()
                     {
                         AiCommunicatorConfig = aiCommConfig.Value
                     };
        UpdateSettingsCommand = ReactiveCommand.CreateFromTask(async x =>
                                                               {
                                                                   await repo.Update(FrontendConfig);
                                                                   screen.Router.NavigateBack.Execute();
                                                               });
        Subscriptions.Add(this.WhenPropertyChanged(x => x.SelectedInputAudioSource)
                              .Subscribe(x => GlobalAppState.SelectedInputAudioDevice = x.Value));
        Subscriptions.Add(this.WhenPropertyChanged(x => x.SelectedOutputAudioSource)
                              .Subscribe(x => GlobalAppState.SelectedOutputAudioDevice = x.Value));
    }

    public SettingsViewModel()
    {
        InputAudioDevices  = new();
        OutputAudioDevices = new();
    }

    public void Refresh()
    {
        InputAudioDevices  = AudioInterfaceManager.InputDevices;
        OutputAudioDevices = AudioInterfaceManager.OutputDevices;
    }

    
    private AudioInterface?             _selectedOutputAudioSource;
    private AudioInterface?             _selectedInputAudioSource;
    private FrontendConfig                  _frontendConfig;
    private IAudioInterfaceManager      AudioInterfaceManager { get; }
    private List<IDisposable>           Subscriptions         { get; set; } = new();

    public void Dispose()
    {
        UpdateSettingsCommand.Dispose();
        foreach (var sub in Subscriptions)
        {
            sub.Dispose();
        }
    }
}