using Models.Configs;
using ReactiveUI;

namespace Models;

public class FrontendConfig 
{
    private AiCommunicatorConfig _aiCommunicatorConfig;

    public AiCommunicatorConfig AiCommunicatorConfig
    {
        get => _aiCommunicatorConfig;
        set => _aiCommunicatorConfig = value;
    }
}