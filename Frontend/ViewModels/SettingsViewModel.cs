using System;
using System.Collections.Generic;
using System.Reactive;
using DynamicData.Binding;
using Frontend.Globals;
using Microsoft.Extensions.Options;
using Models;
using ReactiveUI;
using ServiceImplementations.Configs;
using ServiceInterfaces;

namespace Frontend.ViewModels;

public class SettingsViewModel : ReactiveObject, IRoutableViewModel,IDisposable
{
    public string?                 UrlPathSegment     => "Settings";
    public IScreen                 HostScreen         { get; } = null!;
    public HashSet<AudioInterface> OutputAudioDevices { get; set; }

    public HashSet<AudioInterface> InputAudioDevices { get; set; }

    public CommConfig CommConfig
    {
        get => _commConfig;
        set => this.RaiseAndSetIfChanged(ref _commConfig, value);
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
                             ISettingsRepository<CommConfig> repo,
                             IOptions<AiCommunicatorConfig> aiCommConfig)
    {
        InputAudioDevices     = audioInterfaceManager.InputDevices;
        OutputAudioDevices    = audioInterfaceManager.OutputDevices;
        HostScreen            = screen;
        AudioInterfaceManager = audioInterfaceManager;
        CommConfig = new CommConfig()
                     {
                         WebApiKey     = aiCommConfig.Value.WebApiKey, BaseAddress = aiCommConfig.Value.BaseAddress,
                         WorkspaceSlug = aiCommConfig.Value.WorkspaceSlug
                     };
        UpdateSettingsCommand = ReactiveCommand.CreateFromTask(async x =>
                                                               {
                                                                   await repo.Update(CommConfig);
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
    private CommConfig                  _commConfig;
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