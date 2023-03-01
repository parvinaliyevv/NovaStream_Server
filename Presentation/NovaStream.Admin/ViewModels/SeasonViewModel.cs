namespace NovaStream.Admin.ViewModels;

public class SeasonViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;
    private readonly IAWSStorageManager _awsStorageManager;

    public static bool isCreated = false; // feature

    private int _seasonCount;
    public int SeasonCount
    {
        get => _seasonCount;
        set { _seasonCount = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<Season> _seasons;
    public ObservableCollection<Season> Seasons
    {
        get => _seasons;
        set { _seasons = value; RaisePropertyChanged(); }
    }

    public RelayCommand<string> SearchCommand { get; set; }
    public RelayCommand<Season> DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }


    public SeasonViewModel(AppDbContext dbContext, IStorageManager storageManager, IAWSStorageManager awsStorageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;
        _awsStorageManager = awsStorageManager;

        isCreated = true;

        Initialize();

        SearchCommand = new RelayCommand<string>(pattern => Search(pattern));
        DeleteCommand = new RelayCommand<Season>(season => Delete(season));
        RefreshCommand = new RelayCommand(() => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
    }


    public async Task Initialize()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        _ = MessageBoxService.Show($"Loading seasons...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            Seasons = new ObservableCollection<Season>(_dbContext.Seasons);
            SeasonCount = Seasons.Count;

            Seasons.CollectionChanged += SeasonCountChanged;

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
            var seasons = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.Seasons.ToList() :
            _dbContext.Seasons.ToArray().Where(s => string.Format("{0} S{1:00}", s.SerialName, s.Number).Contains(pattern)).ToList();

            if (Seasons.Count == seasons.Count) return;

            Seasons.Clear();

            seasons.ForEach(s => Seasons.Add(s));
        }
        catch
        {
            await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);
        }
    }

    private async Task Delete(Season season)
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(season);

        _ = MessageBoxService.Show(string.Format("Delete <{0} S{1:00}>...", season.SerialName, season.Number), MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            string message = string.Empty;

            if (season.Number == 1) message = "You can't delete the first season of a serial, but you can't delete an serial!";

            var lastSeasonNumber = _dbContext.Seasons.Where(s => s.SerialName == season.SerialName).Max(e => e.Number);

            if (season.Number < lastSeasonNumber) message = "You can delete only the last season of the serial!";

            if (!string.IsNullOrWhiteSpace(message)) { await MessageBoxService.Show(message, MessageBoxType.Error); return; }

            var episodes = _dbContext.Episodes.Where(e => e.SeasonId == season.Id);

            var imageUrls = new List<string>();
            var videoUrls = new List<string>();

            foreach (var episode in episodes)
            {
                imageUrls.Add(episode.ImageUrl);
                videoUrls.Add(episode.VideoUrl);
            }

            _dbContext.Seasons.Remove(season);
            await _dbContext.SaveChangesAsync();

            if (EpisodeViewModel.isCreated)
            {
                var model = App.ServiceProvider.GetService<EpisodeViewModel>();

                var items = new List<Episode>();

                foreach (var episode in model.Episodes)
                    if (episode.Season is null) items.Add(episode);

                items.ForEach(episode => model.Episodes.Remove(episode));
            }

            Seasons.Remove(season);

            foreach (var imageUrl in imageUrls) _storageManager.DeleteFile(imageUrl);
            foreach (var videoUrl in videoUrls) await _awsStorageManager.DeleteFileAsync(videoUrl);

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

        var model = App.ServiceProvider.GetService<AddSeasonViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    public void SeasonCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => SeasonCount = Seasons.Count;
}
