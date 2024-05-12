using ReactiveUI;

namespace Models;

public class CommConfig : ReactiveObject
{
    private string _baseAddress;
    private string _webApiKey;
    private string _workspaceSlug;

    public string BaseAddress
    {
        get => _baseAddress;
        set => this.RaiseAndSetIfChanged(ref _baseAddress, value);
    }

    public string WebApiKey
    {
        get => _webApiKey;
        set => this.RaiseAndSetIfChanged(ref _webApiKey, value);
    }

    public string WorkspaceSlug
    {
        get => _workspaceSlug;
        set => this.RaiseAndSetIfChanged(ref _workspaceSlug, value);
    }
}