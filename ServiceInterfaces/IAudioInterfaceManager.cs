using System.Collections.Generic;
using InternalDtos;

namespace ServiceInterfaces;

public interface IAudioInterfaceManager
{
    HashSet<AudioInterface> InputDevices  { get; }
    HashSet<AudioInterface> OutputDevices { get; }
}