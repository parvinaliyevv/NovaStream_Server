namespace NovaStream.Admin.ViewModels;

public class EpisodeViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;

    public ObservableCollection<Episode> Episodes { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand<Button> OpenEditDialogHostCommand { get; set; }
    public RelayCommand<Button> DeleteCommand { get; set; }


    public EpisodeViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        Initialize();
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        Episodes = new ObservableCollection<Episode>(_dbContext.Episodes.Include(e => e.Season));

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand<Button>(button => OpenEditDialogHost(button));
        DeleteCommand = new RelayCommand<Button>(button => Delete(button));
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddEpisodeViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(Button button)
    {
        // var serial = button?.DataContext as Serial;
        // 
        // ArgumentNullException.ThrowIfNull(serial);

        var model = App.ServiceProvider.GetService<AddEpisodeViewModel>();

        // model.Serial = serial;
        // model.Season = _dbContext.Seasons.FirstOrDefault(s => s.SerialName == serial.Name);
        // model.Episode = _dbContext.Episodes.FirstOrDefault(e => e.SeasonId == model.Season.Id);
        // 
        await DialogHost.Show(model, "RootDialog");
    }

    private async Task Delete(Button button)
    {
        var episode = button?.DataContext as Episode;

        ArgumentNullException.ThrowIfNull(episode);

        _dbContext.Episodes.Remove(episode);
        await _dbContext.SaveChangesAsync();

        Episodes.Remove(episode);
    }
}
