using ServiceInterfaces;

namespace AnythingLLMCommunicator;

public class MainLoop : BackgroundService
{
    private readonly ILogger<MainLoop> _logger;

    public MainLoop(ILogger<MainLoop> logger, ISpeech speechService, IWebApiCommunicator communicator)
    {
        SpeechService = speechService;
        Communicator  = communicator;
        _logger       = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("Premi qualsiasi Tasto per procedere con il riconoscimento vocale");
            string inputMessage = await SpeechService.VoiceToText();
            var    response     = await Communicator.SendChatRequest( inputMessage);
            Console.WriteLine(response);
            await SpeechService.TextToAudio(response.TextResponse);
            await Task.Delay(1000, stoppingToken);
        }
    }

    private ISpeech             SpeechService { get; init; }
    private IWebApiCommunicator Communicator  { get; init; }
}