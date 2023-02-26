namespace NovaStream.Admin.ViewModels;

public class SeasonViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;
    private readonly IAWSStorageManager _awsStorageManager;

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

    public RelayCommand SearchCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }


    public SeasonViewModel(AppDbContext dbContext, IStorageManager storageManager, IAWSStorageManager awsStorageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;
        _awsStorageManager = awsStorageManager;

        Initialize();
    }


    public async Task Initialize()
    {
        await Task.CompletedTask;

        Seasons = new ObservableCollection<Season>(_dbContext.Seasons);
        SeasonCount = Seasons.Count;

        Seasons.CollectionChanged += SeasonCountChanged;

        SearchCommand = new RelayCommand(sender => Search(sender));
        DeleteCommand = new RelayCommand(sender => Delete(sender));
        RefreshCommand = new RelayCommand(_ => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(_ => OpenAddDialogHost());
    }

    private async Task Search(object sender)
    {
        await Task.CompletedTask;

        var pattern = sender.ToString();

        var seasons = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.Seasons.ToList() :
            _dbContext.Seasons.ToArray().Where(s => string.Format("{0} S{1:00}", s.SerialName, s.Number).Contains(pattern)).ToList();

        if (Seasons.Count == seasons.Count) return;

        Seasons.Clear();

        seasons.ForEach(s => Seasons.Add(s));
    }

    private async Task Delete(object sender)
    {
        var button = sender as Button;
        var season = button?.DataContext as Season;

        ArgumentNullException.ThrowIfNull(season);

        if (season.Number == 1 )
        {
            await MessageBoxService.Show("You can't delete the first season of a serial, but you can't delete an serial!", MessageBoxType.Error);

            return;
        }

        var lastSeasonNumber = _dbContext.Seasons.Where(s => s.SerialName == season.SerialName).Max(e => e.Number);

        if (season.Number < lastSeasonNumber)
        {
            await MessageBoxService.Show("You can delete only the last season of the serial!", MessageBoxType.Error);

            return;
        }

        _ = MessageBoxService.Show(string.Format("Delete <{0} S{1:00}>...", season.SerialName, season.Number), MessageBoxType.Progress);

        var episodes = _dbContext.Episodes.Where(e => e.SeasonId == season.Id);

        foreach (var episode in episodes)
        {
            _storageManager.DeleteFile(episode.ImageUrl);
            await _awsStorageManager.DeleteFileAsync(episode.VideoUrl);
        }

        _dbContext.Seasons.Remove(season);
        await _dbContext.SaveChangesAsync();

        await App.ServiceProvider.GetService<EpisodeViewModel>().Initialize();

        Seasons.Remove(season);

        MessageBoxService.Close();
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddSeasonViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private void SeasonCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => SeasonCount = Seasons.Count;
}
