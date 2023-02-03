namespace NovaStream.Admin.ViewModels;

public class SeasonViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;

    public ObservableCollection<Season> Seasons { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand<Button> OpenEditDialogHostCommand { get; set; }
    public RelayCommand<Button> DeleteCommand { get; set; }


    public SeasonViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        Initialize();
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        Seasons = new ObservableCollection<Season>(_dbContext.Seasons);

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand<Button>(button => OpenEditDialogHost(button));
        DeleteCommand = new RelayCommand<Button>(button => Delete(button));
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddSeasonViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(Button button)
    {
        var season = button?.DataContext as Season;

        ArgumentNullException.ThrowIfNull(season);

        var model = App.ServiceProvider.GetService<AddSeasonViewModel>();

        model.Season = season;

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task Delete(Button button)
    {
        var season = button?.DataContext as Season;

        ArgumentNullException.ThrowIfNull(season);
        
        _dbContext.Seasons.Remove(season);
        await _dbContext.SaveChangesAsync();

        Seasons.Remove(season);
    }
}
