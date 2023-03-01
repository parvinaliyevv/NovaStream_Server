namespace NovaStream.Admin.ViewModels;

public class EpisodeViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;
    private readonly IAWSStorageManager _awsStorageManager;

    public static bool isCreated = false; // feature

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

    public RelayCommand<string> SearchCommand { get; set; }
    public RelayCommand<Episode> DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand<Episode> OpenEditDialogHostCommand { get; set; }


    public EpisodeViewModel(AppDbContext dbContext, IStorageManager storageManager, IAWSStorageManager awsStorageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;
        _awsStorageManager = awsStorageManager;

        isCreated = true;

        Initialize();

        SearchCommand = new RelayCommand<string>(pattern => Search(pattern));
        DeleteCommand = new RelayCommand<Episode>(episode => Delete(episode));
        RefreshCommand = new RelayCommand(() => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand<Episode>(episode => OpenEditDialogHost(episode));
    }


    public async Task Initialize()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        _ = MessageBoxService.Show($"Loading episodes...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            Episodes = new ObservableCollection<Episode>(_dbContext.Episodes.Include(e => e.Season));
            EpisodeCount = Episodes.Count;

            Episodes.CollectionChanged += EpisodeCountChanged;

            MessageBoxService.Close();
        }
        catch
        {
            await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);
        }
    }

    private async Task Search(string pattern)
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        try
        {
            var episodes = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.Episodes.Include(e => e.Season).ToList() :
            _dbContext.Episodes.Include(e => e.Season).ToArray().Where(e =>
                string.Format("{0} S{1:00} E{2:00}", e.Season.SerialName, e.Season.Number, e.Number).Contains(pattern) ||
                string.Format("{0} {1}", e.Season.SerialName, e.Season.Number, e.Name).Contains(pattern)).ToList();

            if (Episodes.Count == episodes.Count) return;

            Episodes.Clear();

            episodes.ForEach(e => Episodes.Add(e));
        }
        catch
        {
            await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);
        }
    }

    private async Task Delete(Episode episode)
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(episode);

        _ = MessageBoxService.Show(string.Format("Delete <{0} S{1:00} E{2:00}>...", episode.Season.SerialName, episode.Season.Number, episode.Number), MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            string message = string.Empty;

            if (episode.Number == 1 && episode.Season.Number == 1) message = "You can't delete the first episode of a serial, but you can't delete an serial!";

            var lastEpisodeNumber = _dbContext.Episodes.Where(e => e.SeasonId == episode.SeasonId).Max(e => e.Number);

            if (episode.Number < lastEpisodeNumber) message = "You can delete only the last episode of the season!";

            if (!string.IsNullOrWhiteSpace(message)) { await MessageBoxService.Show(message, MessageBoxType.Error); return; }

            var videoUrl = episode.VideoUrl;
            var imageUrl = episode.ImageUrl;

            _dbContext.Episodes.Remove(episode);
            await _dbContext.SaveChangesAsync();

            Episodes.Remove(episode);

            await _awsStorageManager.DeleteFileAsync(videoUrl);
            await _storageManager.DeleteFileAsync(imageUrl);

            MessageBoxService.Close();
        }
        catch
        {
            await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);
        }
    }

    private async Task OpenAddDialogHost()
    {
        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        var model = App.ServiceProvider.GetService<AddEpisodeViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(Episode episode)
    {
        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(episode);

        var model = App.ServiceProvider.GetService<EditEpisodeViewModel>();

        model.Episode = episode.Adapt<EpisodeViewModelContent>();
        model.Episode.Serial = _dbContext.Serials.FirstOrDefault(s => s.Name == episode.Season.SerialName);
        model.Episode.Season = episode.Season;

        await DialogHost.Show(model, "RootDialog");
    }

    public void EpisodeCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => EpisodeCount = Episodes.Count;
}
