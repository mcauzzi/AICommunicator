using Models;
using ServiceInterfaces;

namespace ConsoleCommunicator;

public class MainLoop : BackgroundService
{
    public MainLoop(ILogger<MainLoop>      logger, ISpeech speechService, ILLMWebApiCommunicator communicator,
                    IAudioInterfacePicker audioPicker)
    {
        SpeechService = speechService;
        Communicator  = communicator;
        AudioPicker  = audioPicker;
        _logger       = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(100, stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1) Select Input Device");
            Console.WriteLine("2) Select Output Device");
            Console.WriteLine("3) Chat");
            if (int.TryParse(Console.ReadLine(), out var choice))
            {
                await ManageChoice(choice);
            }
            else
            {
                Console.WriteLine("Write a valid choice");
            }

            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task ManageChoice(int choice)
    {
        switch (choice)
        {
            case 1:
                SelectedInputDevice = AudioPicker.GetInputAudioDevice() ?? SelectedInputDevice;
                break;
            case 2:
                SelectedOutputDevice = AudioPicker.GetOutputAudioDevice() ?? SelectedInputDevice;
                break;
            case 3:
                await Chat();
                break;
            default:
                Console.WriteLine("Written number not valid");
                break;
        }
    }

    private async Task Chat()
    {
        try
        {
            var inputMessage = await SpeechService.VoiceToText(SelectedInputDevice);
            if (!string.IsNullOrEmpty(inputMessage))
            {
                var response = await Communicator.SendChatRequest(inputMessage);
                Console.WriteLine(response);
                await SpeechService.TextToAudio(response.TextResponse,SelectedOutputDevice);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during the chat request");
        }
    }

    private          AudioInterface         SelectedInputDevice  { get; set; }
    private          AudioInterface         SelectedOutputDevice { get; set; }
    private          ISpeech                SpeechService        { get; init; }
    private          ILLMWebApiCommunicator Communicator         { get; init; }
    public           IAudioInterfacePicker AudioPicker         { get; }
    private readonly ILogger<MainLoop>      _logger;
}