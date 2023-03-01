namespace NovaStream.Admin.Services;

public interface INavigationService
{
    void NavigateTo<T>() where T : ViewModelBase;
}

public class NavigationService : INavigationService
{
    private readonly IMessenger _messenger;


    public NavigationService(IMessenger messenger)
    {
        _messenger = messenger;
    }


    public void NavigateTo<T>() where T : ViewModelBase
        => _messenger.Send(new NavigationMessage(typeof(T)));
}
