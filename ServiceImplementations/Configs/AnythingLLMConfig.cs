namespace ServiceImplementations.Configs;

// ReSharper disable once InconsistentNaming
public class AnythingLLMConfig
{
    public required string BaseAddress   { get; set; }
    public required string WebApiKey     { get; set; }
    public required string WorkspaceSlug { get; set; }
}