namespace NovaStream.Admin.ViewModels;

public class EpisodeViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;
    private readonly IAWSStorageManager _awsStorageManager;

    public ObservableCollection<Episode> Episodes { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand<Button> OpenEditDialogHostCommand { get; set; }
    public RelayCommand<Button> DeleteCommand { get; set; }


    public EpisodeViewModel(AppDbContext dbContext, IStorageManager storageManager, IAWSStorageManager awsStorageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;
        _awsStorageManager = awsStorageManager;

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
        var episode = button?.DataContext as Episode;

        ArgumentNullException.ThrowIfNull(episode);

        var model = App.ServiceProvider.GetService<AddEpisodeViewModel>();

        model.Episode = episode;

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task Delete(Button button)
    {
        var episode = button?.DataContext as Episode;

        ArgumentNullException.ThrowIfNull(episode);

        await _awsStorageManager.DeleteFileAsync(episode.VideoUrl);
        await _storageManager.DeleteFileAsync(episode.ImageUrl);

        _dbContext.Episodes.Remove(episode);
        await _dbContext.SaveChangesAsync();

        Episodes.Remove(episode);
    }
}
