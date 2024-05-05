using System;
using System.Collections.Generic;
using System.Linq;
using InternalDtos;
using Microsoft.Extensions.Logging;
using NAudio.CoreAudioApi;
using ServiceInterfaces;

namespace ServiceImplementations;

public class AudioInterfaceManager : IAudioInterfaceManager
{
    public AudioInterfaceManager(ILogger<AudioInterfaceManager> logger)
    {
        Logger = logger;
    }


    public HashSet<AudioInterface>        OutputDevices => GetOutPutDevices();
    public HashSet<AudioInterface>        InputDevices  => GetInputDevices();
    public ILogger<AudioInterfaceManager> Logger        { get; }

    private MMDeviceEnumerator AudioDeviceEnumerator { get; set; } = new MMDeviceEnumerator();

    private HashSet<AudioInterface> GetOutPutDevices()
    {
        return FindOutputDevices();
    }

    private HashSet<AudioInterface> GetInputDevices()
    {
        return FindInputDevices();
    }

    private HashSet<AudioInterface> FindOutputDevices()
    {
        return AudioDeviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)
                                    .Select((x) => new AudioInterface()
                                                   { Id = x.ID, Name = x.FriendlyName }).ToHashSet();
    }

    private HashSet<AudioInterface> FindInputDevices()
    {
        return AudioDeviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)
                                    .Select((x) => new AudioInterface() { Id = x.ID, Name = x.FriendlyName })
                                    .ToHashSet();
    }
}