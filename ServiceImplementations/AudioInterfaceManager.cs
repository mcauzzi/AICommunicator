using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InternalDtos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NAudio.CoreAudioApi;
using ServiceInterfaces;

namespace ServiceImplementations;

public class AudioInterfaceManager : BackgroundService, IAudioInterfaceManager
{
    public ILogger<AudioInterfaceManager> Logger { get; }

    public AudioInterfaceManager(ILogger<AudioInterfaceManager> logger)
    {
        Logger = logger;
    }

    public HashSet<AudioInterface> InputDevices  { get; set; } = new();
    public HashSet<AudioInterface> OutputDevices { get; set; } = new();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var enumerator = new MMDeviceEnumerator();
            ManageInputDevices(enumerator);
            ManageOutputDevices(enumerator);

            await Task.Delay(5000, stoppingToken);
        }
    }

    private void ManageOutputDevices(MMDeviceEnumerator enumerator)
    {
        try
        {
            var currentOutputDevices = FindOutputDevices(enumerator);
            foreach (var endpoint in currentOutputDevices)
            {
                if (OutputDevices.Add(endpoint))
                {
                    Logger.LogInformation("Found new output audio device {AudioDevice}",
                                          endpoint.Name);
                }
            }

            foreach (var endpoint in OutputDevices)
            {
                if (!currentOutputDevices.Contains(endpoint))
                {
                    OutputDevices.Remove(endpoint);
                    Logger.LogInformation("Output Audio device removed {AudioDevice}",
                                          endpoint.Name);
                }
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while managing Output Devices");
        }
    }

    private void ManageInputDevices(MMDeviceEnumerator enumerator)
    {
        try
        {
            var currentInputDevices = FindInputDevices(enumerator);
            foreach (var endpoint in currentInputDevices)
            {
                if (InputDevices.Add(endpoint))
                {
                    Logger.LogInformation("Found new Input audio device {AudioDevice}",
                                          endpoint.Name);
                }
            }

            foreach (var endpoint in InputDevices)
            {
                if (!currentInputDevices.Contains(endpoint))
                {
                    InputDevices.Remove(endpoint);
                    Logger.LogInformation("Input Audio device removed {AudioDevice}",
                                          endpoint.Name);
                }
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while managing Input Devices");
        }
    }

    private static List<AudioInterface> FindOutputDevices(MMDeviceEnumerator enumerator)
    {
        return enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)
                         .Select((x) => new AudioInterface()
                                        { Id = x.ID, Name = x.FriendlyName }).ToList();
    }

    private static IList<AudioInterface> FindInputDevices(MMDeviceEnumerator enumerator)
    {
        return enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)
                         .Select((x) => new AudioInterface() { Id = x.ID, Name = x.FriendlyName }).ToList();
    }
}