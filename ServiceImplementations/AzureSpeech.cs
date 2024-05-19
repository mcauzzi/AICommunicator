using System;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Logging;
using Models;
using ServiceImplementations.Exceptions;
using ServiceInterfaces;

namespace ServiceImplementations;

public class AzureSpeech : ISpeech, IDisposable
{
    public AzureSpeech(ILogger<AzureSpeech> logger)
    {
        Logger       = logger;
        SpeechConfig = SpeechConfig.FromSubscription(SpeechKey, SpeechRegion);
        LoggerScope = Logger.BeginScope(new Dictionary<string, string>
                                        {
                                            [nameof(SpeechConfig.SpeechSynthesisLanguage)] =
                                                SpeechConfig.SpeechSynthesisLanguage,
                                            [nameof(SpeechConfig.SpeechSynthesisVoiceName)] =
                                                SpeechConfig.SpeechSynthesisVoiceName
                                        });
        SpeechConfig.SpeechSynthesisVoiceName = "it-IT-CalimeroNeural";
        SpeechConfig.SpeechSynthesisLanguage  = "it-IT";
    }

    public IDisposable? LoggerScope { get; set; }

    private SpeechConfig? SpeechConfig { get; set; }

    string OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
    {
        switch (speechRecognitionResult.Reason)
        {
            case ResultReason.RecognizedSpeech:
                Logger.LogInformation("Recognized text {RecognizedText}", speechRecognitionResult.Text);
                return speechRecognitionResult.Text;
            case ResultReason.NoMatch:
                Logger.LogInformation("Speech could not be recognized");
                break;
            case ResultReason.Canceled:
                var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                Logger.LogInformation("Speech recognition cancelled, Reason={CancellationReason}", cancellation.Reason);

                if (cancellation.Reason == CancellationReason.Error)
                {
                    Logger.LogInformation("ErrorCode:{CancellationErrorCode}, ErrorDetails:{CancellationErrorDetails}",
                                          cancellation.ErrorCode, cancellation.ErrorDetails);
                }

                break;
        }

        return "";
    }

    public async Task<string> VoiceToText(AudioInterface inputDevice)
    {
        SpeechConfig.SpeechRecognitionLanguage = "it-IT";
        //"74929fe5-542f-4f6f-a9fd-81a4bea18fe1"
        using var audioConfig      = AudioConfig.FromMicrophoneInput(inputDevice.Id);
        using var speechRecognizer = new SpeechRecognizer(SpeechConfig, audioConfig);
        Console.WriteLine("Speak into your microphone.");
        Logger.LogInformation("Sending Speech-to-text request to azure");
        var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
        return OutputSpeechRecognitionResult(speechRecognitionResult);
    }

    public async Task TextToAudio(string text, AudioInterface selectedOutputDevice)
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }
        using var audioConfig           = AudioConfig.FromSpeakerOutput(selectedOutputDevice.Id);
        using var speechSynthesizer     = new SpeechSynthesizer(SpeechConfig,audioConfig);
        var       speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
    }

    public void Dispose()
    {
        LoggerScope?.Dispose();
    }
    
    private ILogger<AzureSpeech> Logger { get; }

    private static readonly string SpeechKey =
        Environment.GetEnvironmentVariable("SPEECH_KEY") ?? throw new AzureSpeechKeyNotSetException();

    private static readonly string SpeechRegion = Environment.GetEnvironmentVariable("SPEECH_REGION") ??
                                                   throw new AzureSpeechRegionNotSetException();
}