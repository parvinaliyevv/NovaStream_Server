namespace NovaStream.Admin.ViewModels;

public class EpisodeViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;
    private readonly IAWSStorageManager _awsStorageManager;

    private int _episodeCount;
    public int EpisodeCount
    {
        get => _episodeCount;
        set { _episodeCount = value; RaisePropertyChanged(); }
    }

    public ObservableCollection<Episode> Episodes { get; set; }

    public RelayCommand SearchCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand OpenEditDialogHostCommand { get; set; }


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
        EpisodeCount = Episodes.Count;

        Episodes.CollectionChanged += EpisodeCountChanged;

        SearchCommand = new RelayCommand(sender => Search(sender));
        DeleteCommand = new RelayCommand(sender => Delete(sender));
        
        OpenAddDialogHostCommand = new RelayCommand(_ => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand(sender => OpenEditDialogHost(sender));
    }

    private async Task Search(object sender)
    {
        await Task.CompletedTask;

        var pattern = sender.ToString();

        var episodes = string.IsNullOrWhiteSpace(pattern)
            ? _dbContext.Episodes.ToList() : _dbContext.Episodes.Where(e => e.Name.Contains(pattern)).ToList();

        if (Episodes.Count == episodes.Count) return;
        
        Episodes.Clear();

        episodes.ForEach(e => Episodes.Add(e));
    }
    
    private async Task Delete(object sender)
    {
        var button = sender as Button;
        var episode = button?.DataContext as Episode;

        ArgumentNullException.ThrowIfNull(episode);

        await _awsStorageManager.DeleteFileAsync(episode.VideoUrl);
        await _storageManager.DeleteFileAsync(episode.ImageUrl);

        _dbContext.Episodes.Remove(episode);
        await _dbContext.SaveChangesAsync();

        Episodes.Remove(episode);
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddEpisodeViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(object sender)
    {
        var button = sender as Button;
        var episode = button?.DataContext as Episode;

        ArgumentNullException.ThrowIfNull(episode);

        var model = App.ServiceProvider.GetService<AddEpisodeViewModel>();

        model.Episode = episode.Adapt<UploadEpisodeModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private void EpisodeCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => EpisodeCount = Episodes.Count;
}
