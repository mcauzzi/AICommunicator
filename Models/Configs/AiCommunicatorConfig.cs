namespace Models.Configs;

// ReSharper disable once InconsistentNaming
public class AiCommunicatorConfig
{
    public required string BaseAddress   { get; set; }
    public required string WebApiKey     { get; set; }
    public required string WorkspaceSlug { get; set; }
}