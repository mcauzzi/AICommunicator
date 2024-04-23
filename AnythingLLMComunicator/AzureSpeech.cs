using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using NAudio.Wave;

namespace AnythingLLMComunicator;

public class AzureSpeech
{
    private static readonly string SPEECH_KEY    = Environment.GetEnvironmentVariable("SPEECH_KEY");
    private static readonly string SPEECH_REGION = Environment.GetEnvironmentVariable("SPEECH_REGION");

    public AzureSpeech()
    {
        SpeechConfig                          = SpeechConfig.FromSubscription(SPEECH_KEY, SPEECH_REGION);
        SpeechConfig.SpeechSynthesisVoiceName = "it-IT-CalimeroNeural";
        SpeechConfig.SpeechSynthesisLanguage  = "it-IT";
    }

    private SpeechConfig? SpeechConfig { get; set; }

    static string OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
    {
        switch (speechRecognitionResult.Reason)
        {
            case ResultReason.RecognizedSpeech:
                Console.WriteLine($"RECOGNIZED: Text={speechRecognitionResult.Text}");
                return speechRecognitionResult.Text;
                break;
            case ResultReason.NoMatch:
                Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                break;
            case ResultReason.Canceled:
                var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                if (cancellation.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                    Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                }

                break;
        }

        return "";
    }

    public async Task<string> GetMessage()
    {
        SpeechConfig.SpeechRecognitionLanguage = "it-IT";
        //"74929fe5-542f-4f6f-a9fd-81a4bea18fe1"
        using var audioConfig      = AudioConfig.FromDefaultMicrophoneInput();
        using var speechRecognizer = new SpeechRecognizer(SpeechConfig, audioConfig);

        Console.WriteLine("Speak into your microphone.");
        var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
        return OutputSpeechRecognitionResult(speechRecognitionResult);
    }

    public async Task SpeakText(string text)
    {
        SpeechConfig.SpeechSynthesisVoiceName = "en-US-AvaMultilingualNeural";

        using (var speechSynthesizer = new SpeechSynthesizer(SpeechConfig))
        {
            var       speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
            using var stream                = AudioDataStream.FromResult(speechSynthesisResult);
            await stream.SaveToWaveFileAsync("./lastTTSResult.wav");
            using(var audioFile = new AudioFileReader("./lastTTSResult.wav"))
            using(var outputDevice = new WaveOutEvent())
            {
                outputDevice.DeviceNumber = 1;
                outputDevice.Init(audioFile);
               
                outputDevice.Play();
            }
        }
    }
}