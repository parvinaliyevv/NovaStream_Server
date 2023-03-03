namespace NovaStream.Admin.ViewModels;

public class SerialViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;
    private readonly IAWSStorageManager _awsStorageManager;

    private int _serialCount;
    public int SerialCount
    {
        get => _serialCount;
        set { _serialCount = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<Serial> _serials;
    public ObservableCollection<Serial> Serials
    {
        get => _serials;
        set { _serials = value; RaisePropertyChanged(); }
    }

    public RelayCommand<string> SearchCommand { get; set; }
    public RelayCommand<Serial> DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand<Serial> OpenEditDialogHostCommand { get; set; }


    public SerialViewModel(AppDbContext dbContext, IStorageManager storageManager, IAWSStorageManager awsStorageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;
        _awsStorageManager = awsStorageManager;

        Initialize();

        SearchCommand = new RelayCommand<string>(pattern => Search(pattern));
        DeleteCommand = new RelayCommand<Serial>(serial => Delete(serial));
        RefreshCommand = new RelayCommand(() => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand<Serial>(serial => OpenEditDialogHost(serial));
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }
        
        _ = MessageBoxService.Show($"Loading serials...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            Serials = new ObservableCollection<Serial>(_dbContext.Serials.Include(s => s.Director));
            SerialCount = Serials.Count;

            Serials.CollectionChanged += SerialCountChanged;

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
            var serials = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.Serials.Include(s => s.Director).ToList() :
            _dbContext.Serials.Include(s => s.Director).Where(s => s.Name.Contains(pattern)).ToList();

            if (Serials.Count == serials.Count) return;

            Serials.Clear();

            serials.ForEach(s => Serials.Add(s));
        }
        catch
        {
            await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);
        }
    }

    private async Task Delete(Serial serial)
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(serial);

        _ = MessageBoxService.Show($"Delete <{serial.Name}>...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            var episodes = _dbContext.Episodes.Include(e => e.Season).Where(e => e.Season.SerialName == serial.Name);

            var trailerUrl = serial.TrailerUrl;
            var imageUrl = serial.ImageUrl;
            var searchImageUrl = serial.SearchImageUrl;

            var episodeImageUrls = new List<string>();
            var episodeVideoUrls = new List<string>();

            foreach (var episode in episodes)
            {
                episodeImageUrls.Add(episode.ImageUrl);
                episodeVideoUrls.Add(episode.VideoUrl);
            }

            _dbContext.Serials.Remove(serial);
            await _dbContext.SaveChangesAsync();

            foreach (var episodeImageUrl in episodeImageUrls) _storageManager.DeleteFile(episodeImageUrl);
            foreach (var episodeVideoUrl in episodeVideoUrls) await _awsStorageManager.DeleteFileAsync(episodeVideoUrl);

            await _storageManager.DeleteFileAsync(trailerUrl);
            await _storageManager.DeleteFileAsync(imageUrl);
            await _storageManager.DeleteFileAsync(searchImageUrl);

            if (SeasonViewModel.isCreated)
            {
                var model = App.ServiceProvider.GetService<SeasonViewModel>();

                var items = new List<Season>();
                
                foreach (var season in model.Seasons)
                    if (season.SerialName == serial.Name) items.Add(season);

                items.ForEach(season => model.Seasons.Remove(season));
            }
            if (EpisodeViewModel.isCreated)
            {
                var model = App.ServiceProvider.GetService<EpisodeViewModel>();

                var items = new List<Episode>();

                foreach (var episode in model.Episodes)
                    if (episode.Season is null) items.Add(episode);

                items.ForEach(episode => model.Episodes.Remove(episode));
            }

            Serials.Remove(serial);

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

        var model = App.ServiceProvider.GetService<AddSerialViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(Serial serial)
    {
        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(serial);

        var model = App.ServiceProvider.GetService<AddSerialViewModel>();

        model.Serial = serial.Adapt<SerialViewModelContent>();
        model.Serial.Director = serial.Director;
        model.IsEdit = true;

        if (serial is not null)
        {
            var season = await _dbContext.Seasons.FirstOrDefaultAsync(s => s.SerialName == serial.Name && s.Number == 1);

            model.Season = season.Adapt<SeasonViewModelContent>();

            if (model.Season is not null)
            {
                var episode = await _dbContext.Episodes.FirstOrDefaultAsync(e => e.SeasonId == season.Id && e.Number == 1);

                model.Episode = episode.Adapt<EpisodeViewModelContent>();
            }
        }

        await DialogHost.Show(model, "RootDialog");
    }

    private void SerialCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => SerialCount = Serials.Count;
}
