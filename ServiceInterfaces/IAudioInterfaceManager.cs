using System.Collections.Generic;
using InternalDtos;
using Microsoft.Extensions.Hosting;

namespace ServiceInterfaces;

public interface IAudioInterfaceManager
{
    HashSet<AudioInterface> InputDevices  { get; }
    HashSet<AudioInterface> OutputDevices { get; }
}