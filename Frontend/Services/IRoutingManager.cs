using ReactiveUI;

namespace Frontend.Services;

public interface IRoutingManager
{
    public IRoutableViewModel? GetViewModelFromPath(string s);
    void               RegisterMainScreen(IScreen  mainScreen);
}