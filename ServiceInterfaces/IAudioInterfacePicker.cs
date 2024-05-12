using Models;
using Microsoft.Extensions.Hosting;

namespace ServiceInterfaces;

public interface IAudioInterfacePicker
{
    public AudioInterface? GetInputAudioDevice();
    public AudioInterface? GetOutputAudioDevice();
}