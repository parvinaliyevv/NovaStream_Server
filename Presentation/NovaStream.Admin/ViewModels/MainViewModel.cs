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
        SoonViewCommand = new RelayCommand(() => _navigationService.NavigateTo<SoonViewModel>());
        SerialViewCommand = new RelayCommand(() => _navigationService.NavigateTo<SerialViewModel>());
        SeasonViewCommand = new RelayCommand(() => _navigationService.NavigateTo<SeasonViewModel>());
        EpisodeViewCommand = new RelayCommand(() => _navigationService.NavigateTo<EpisodeViewModel>());
        ActorViewCommand = new RelayCommand(() => _navigationService.NavigateTo<ActorViewModel>());
        ProducerViewCommand = new RelayCommand(() => _navigationService.NavigateTo<ProducerViewModel>());
        GenreViewCommand = new RelayCommand(() => _navigationService.NavigateTo<GenreViewModel>());
    }


    public RelayCommand MovieViewCommand { get; set; }
    public RelayCommand SoonViewCommand { get; set; }
    public RelayCommand SerialViewCommand { get; set; }
    public RelayCommand SeasonViewCommand { get; set; }
    public RelayCommand EpisodeViewCommand { get; set; }
    public RelayCommand ActorViewCommand { get; set; }
    public RelayCommand ProducerViewCommand { get; set; }
    public RelayCommand GenreViewCommand { get; set; }
}
