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

    public ObservableCollection<Serial> Serials { get; set; }

    public RelayCommand SearchCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand OpenEditDialogHostCommand { get; set; }


    public SerialViewModel(AppDbContext dbContext, IStorageManager storageManager, IAWSStorageManager awsStorageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;
        _awsStorageManager = awsStorageManager;

        Initialize();
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        Serials = new ObservableCollection<Serial>(_dbContext.Serials.Include(s => s.Producer));
        SerialCount = Serials.Count;

        Serials.CollectionChanged += SerialCountChanged;

        SearchCommand = new RelayCommand(sender => Search(sender));
        DeleteCommand = new RelayCommand(sender => Delete(sender));
        
        OpenAddDialogHostCommand = new RelayCommand(_ => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand(sender => OpenEditDialogHost(sender));
    }

    private async Task Search(object sender)
    {
        await Task.CompletedTask;

        var pattern = sender.ToString();

        var serials = string.IsNullOrWhiteSpace(pattern)
            ? _dbContext.Serials.ToList() : _dbContext.Serials.Where(s => s.Name.Contains(pattern)).ToList();

        if (Serials.Count == serials.Count) return;

        Serials.Clear();

        serials.ForEach(s => Serials.Add(s));
    }

    private async Task Delete(object sender)
    {
        var button = sender as Button;
        var serial = button?.DataContext as Serial;

        ArgumentNullException.ThrowIfNull(serial);

        var episodes = _dbContext.Episodes.Include(e => e.Season)
            .Where(e => e.Season.SerialName == serial.Name);

        foreach (var episode in episodes)
        {
            await _storageManager.DeleteFileAsync(episode.ImageUrl);
            await _awsStorageManager.DeleteFileAsync(episode.VideoUrl);
        }

        await _storageManager.DeleteFileAsync(serial.TrailerUrl);
        await _storageManager.DeleteFileAsync(serial.ImageUrl);
        await _storageManager.DeleteFileAsync(serial.SearchImageUrl);

        var observableSeasons = App.ServiceProvider.GetService<SeasonViewModel>().Seasons;
        var observableEpisodes = App.ServiceProvider.GetService<EpisodeViewModel>().Episodes;

        observableSeasons.Clear();
        observableEpisodes.Clear();

        _dbContext.Seasons.ToList().ForEach(s => observableSeasons.Add(s));
        _dbContext.Episodes.ToList().ForEach(e => observableEpisodes.Add(e));

        _dbContext.Serials.Remove(serial);
        await _dbContext.SaveChangesAsync();

        Serials.Remove(serial);
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddSerialViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(object sender)
    {
        var button = sender as Button;
        var serial = button?.DataContext as Serial;

        ArgumentNullException.ThrowIfNull(serial);

        var model = App.ServiceProvider.GetService<AddSerialViewModel>();

        model.Serial = serial.Adapt<UploadSerialModel>();
        model.Serial.Producer = serial.Producer;

        if (serial is not null)
        {
            var season = await _dbContext.Seasons.FirstOrDefaultAsync(s => s.SerialName == serial.Name && s.Number == 1);

            model.Season = season.Adapt<UploadSeasonModel>();

            if (model.Season is not null)
            {
                var episode = await _dbContext.Episodes.FirstOrDefaultAsync(e => e.SeasonId == season.Id && e.Number == 1);

                model.Episode = episode.Adapt<UploadEpisodeModel>();
            }
        }

        await DialogHost.Show(model, "RootDialog");
    }

    private void SerialCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => SerialCount = Serials.Count;
}
