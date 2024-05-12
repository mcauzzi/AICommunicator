using Models;

namespace Frontend.Globals;

public static class GlobalAppState
{
    public static AudioInterface SelectedInputAudioDevice  { get; set; }
    public static AudioInterface SelectedOutputAudioDevice { get; set; }
}