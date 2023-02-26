namespace NovaStream.Admin.Services;

public interface INavigationService
{
    void NavigateTo<T>() where T : ViewModelBase;
}
