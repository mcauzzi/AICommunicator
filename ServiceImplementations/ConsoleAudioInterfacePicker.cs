using System;
using System.Collections.Generic;
using System.Linq;
using InternalDtos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceInterfaces;

namespace ServiceImplementations;

public class ConsoleAudioInterfacePicker : IAudioInterfacePicker
{
    private readonly AudioInterfaceManager                _audioInterfaceManager;
    public           ILogger<ConsoleAudioInterfacePicker> Logger                { get; }
    public           IAudioInterfaceManager               AudioInterfaceManager { get; }

    public AudioInterface? GetInputAudioDevice()
    {
        AudioInterface? selectedDevice = null;
        while (selectedDevice == null)
        {
            Console.WriteLine("Select an audio device, q to exit");
            PrintDevices(AudioInterfaceManager.InputDevices);

            var selectedId = Console.ReadLine();
            if (selectedId?.ToLower() == "q")
            {
                return null;
            }

            if (int.TryParse(selectedId, out var num))
            {
                selectedDevice = AudioInterfaceManager.InputDevices.ElementAtOrDefault(num);
                if (selectedDevice == null)
                {
                    Console.WriteLine("Written number is not in the interface list");
                }
                else
                {
                    Logger.LogInformation("New input audio device selected! {AudioDevice}", selectedDevice.Name);
                }
            }
            else
            {
                Console.WriteLine("Write a valid number");
            }
        }

        return selectedDevice;
    }

    private void PrintDevices(HashSet<AudioInterface> devices)
    {
        for (int i = 0; i < devices.Count; i++)
        {
            var device = devices.ElementAt(i);
            Console.WriteLine($"{i}) {device.Name}");
        }
    }

    public AudioInterface? GetOutputAudioDevice()
    {
        AudioInterface? selectedDevice = null;
        while (selectedDevice == null)
        {
            Console.WriteLine("Select an audio device, q to exit");
            PrintDevices(AudioInterfaceManager.OutputDevices);
            var selectedId = Console.ReadLine();
            if (selectedId?.ToLower() == "q")
            {
                return null;
            }

            if (int.TryParse(selectedId, out var num))
            {
                selectedDevice = AudioInterfaceManager.OutputDevices.ElementAtOrDefault(num);
                if (selectedDevice == null)
                {
                    Console.WriteLine("Written number is not in the interface list");
                }
                else
                {
                    Logger.LogInformation("New output audio device selected! {AudioDevice}", selectedDevice.Name);
                }
            }
            else
            {
                Console.WriteLine("Write a valid number");
            }
        }

        return selectedDevice;
    }
    
    public ConsoleAudioInterfacePicker(ILogger<ConsoleAudioInterfacePicker> logger,IAudioInterfaceManager audioInterfaceManager)
    {
        Logger                     = logger;
        AudioInterfaceManager = audioInterfaceManager;
    }
}