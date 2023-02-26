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

    private ObservableCollection<Episode> _episodes;
    public ObservableCollection<Episode> Episodes
    {
        get => _episodes;
        set { _episodes = value; RaisePropertyChanged(); }
    }

    public RelayCommand SearchCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand OpenEditDialogHostCommand { get; set; }


    public EpisodeViewModel(AppDbContext dbContext, IStorageManager storageManager, IAWSStorageManager awsStorageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;
        _awsStorageManager = awsStorageManager;

        Initialize();
    }


    public async Task Initialize()
    {
        await Task.CompletedTask;

        Episodes = new ObservableCollection<Episode>(_dbContext.Episodes.Include(e => e.Season));
        EpisodeCount = Episodes.Count;

        Episodes.CollectionChanged += EpisodeCountChanged;

        SearchCommand = new RelayCommand(sender => Search(sender));
        DeleteCommand = new RelayCommand(sender => Delete(sender));
        RefreshCommand = new RelayCommand(_ => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(_ => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand(sender => OpenEditDialogHost(sender));
    }

    private async Task Search(object sender)
    {
        await Task.CompletedTask;

        var pattern = sender.ToString();

        var episodes = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.Episodes.Include(e => e.Season).ToList() :
            _dbContext.Episodes.Include(e => e.Season).ToArray().Where(e => 
                string.Format("{0} S{1:00} E{2:00}", e.Season.SerialName, e.Season.Number, e.Number).Contains(pattern) ||
                string.Format("{0} {1}", e.Season.SerialName, e.Season.Number, e.Name).Contains(pattern)).ToList();

        if (Episodes.Count == episodes.Count) return;

        Episodes.Clear();

        episodes.ForEach(e => Episodes.Add(e));
    }

    private async Task Delete(object sender)
    {
        var button = sender as Button;
        var episode = button?.DataContext as Episode;

        ArgumentNullException.ThrowIfNull(episode);

        if (episode.Number == 1 && episode.Season.Number == 1)
        {
            await MessageBoxService.Show("You can't delete the first episode of a serial, but you can't delete an serial!", MessageBoxType.Error);

            return;
        }

        var lastEpisodeNumber = _dbContext.Episodes.Where(e => e.SeasonId == episode.SeasonId).Max(e => e.Number);

        if (episode.Number < lastEpisodeNumber)
        {
            await MessageBoxService.Show("You can delete only the last episode of the season!", MessageBoxType.Error);

            return;
        }

        _ = MessageBoxService.Show(string.Format("Delete <{0} S{1:00} E{1:00}>...", episode.Season.SerialName, episode.Season.Number, episode.Number), MessageBoxType.Progress);

        await _awsStorageManager.DeleteFileAsync(episode.VideoUrl);
        await _storageManager.DeleteFileAsync(episode.ImageUrl);

        _dbContext.Episodes.Remove(episode);
        await _dbContext.SaveChangesAsync();

        Episodes.Remove(episode);

        MessageBoxService.Close();
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

        var model = App.ServiceProvider.GetService<EditEpisodeViewModel>();

        model.Episode = episode.Adapt<UploadEpisodeModel>();
        model.Episode.Serial = _dbContext.Serials.FirstOrDefault(s => s.Name == episode.Season.SerialName);
        model.Episode.Season = episode.Season;

        await DialogHost.Show(model, "RootDialog");
    }

    private void EpisodeCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => EpisodeCount = Episodes.Count;
}
