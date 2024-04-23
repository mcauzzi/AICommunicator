using System.Threading.Tasks;

namespace ServiceInterfaces;

public interface ISpeech
{
    Task<string> VoiceToText();
    Task         TextToAudio(string text);
}