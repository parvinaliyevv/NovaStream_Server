namespace NovaStream.Admin.ViewModels;

public class MainViewModel : ViewModelBase
{
    private INavigationService _navigationService;

    private ViewModelBase _currentViewModel;
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => Set(ref _currentViewModel, value);
    }


    public MainViewModel(INavigationService navigationService, IMessenger messenger)
    {
        _navigationService = navigationService;
        
        messenger.Register<NavigationMessage>(this, message => CurrentViewModel = App.ServiceProvider.GetService(message.ViewModelType) as ViewModelBase);

        MovieViewCommand = new RelayCommand(() => _navigationService.NavigateTo<MovieViewModel>());
        SerialViewCommand = new RelayCommand(() => _navigationService.NavigateTo<SerialViewModel>());
    }


    public RelayCommand MovieViewCommand { get; set; }
    public RelayCommand SerialViewCommand { get; set; }
}
