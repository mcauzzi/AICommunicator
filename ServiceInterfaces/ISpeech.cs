using System.Threading.Tasks;
using InternalDtos;

namespace ServiceInterfaces;

public interface ISpeech
{
    Task<string> VoiceToText(AudioInterface selectedInputDevice);
    Task         TextToAudio(string text, AudioInterface selectedOutputDevice);
}