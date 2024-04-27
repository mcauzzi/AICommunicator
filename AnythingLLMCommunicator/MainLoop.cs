using ServiceInterfaces;

namespace AnythingLLMCommunicator;

public class MainLoop : BackgroundService
{
    private readonly ILogger<MainLoop> _logger;

    public MainLoop(ILogger<MainLoop> logger, ISpeech speechService, ILLMWebApiCommunicator communicator)
    {
        SpeechService = speechService;
        Communicator  = communicator;
        _logger       = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("Press any key to start chatting with the AI");
            Console.ReadKey();
            try
            {
                string inputMessage = await SpeechService.VoiceToText();
                if (!string.IsNullOrEmpty(inputMessage))
                {
                    var response = await Communicator.SendChatRequest(inputMessage);
                    Console.WriteLine(response);
                    await SpeechService.TextToAudio(response.TextResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error during the chat request");
            }
            await Task.Delay(1000, stoppingToken);
        }
    }

    private ISpeech             SpeechService { get; init; }
    private ILLMWebApiCommunicator Communicator  { get; init; }
}