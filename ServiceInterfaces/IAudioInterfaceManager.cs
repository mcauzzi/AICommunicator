using System.Collections.Generic;
using Models;

namespace ServiceInterfaces;

public interface IAudioInterfaceManager
{
    HashSet<AudioInterface> InputDevices  { get; }
    HashSet<AudioInterface> OutputDevices { get; }
}