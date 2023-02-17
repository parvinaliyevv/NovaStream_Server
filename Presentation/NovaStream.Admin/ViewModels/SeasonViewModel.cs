namespace NovaStream.Admin.ViewModels;

public class SeasonViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;

    private int _seasonCount;
    public int SeasonCount
    {
        get => _seasonCount;
        set { _seasonCount = value; RaisePropertyChanged(); }
    }

    public ObservableCollection<Season> Seasons { get; set; }

    public RelayCommand SearchCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand OpenEditDialogHostCommand { get; set; }


    public SeasonViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        Initialize();
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        Seasons = new ObservableCollection<Season>(_dbContext.Seasons);
        SeasonCount = Seasons.Count;

        Seasons.CollectionChanged += SeasonCountChanged;

        SearchCommand = new RelayCommand(sender => Search(sender));
        DeleteCommand = new RelayCommand(sender => Delete(sender));

        OpenAddDialogHostCommand = new RelayCommand(_ => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand(sender => OpenEditDialogHost(sender));
    }

    private async Task Search(object sender)
    {
        await Task.CompletedTask;

        var pattern = sender.ToString();

        var seasons = string.IsNullOrWhiteSpace(pattern)
            ? _dbContext.Seasons.ToList() : _dbContext.Seasons.Where(s => s.SerialName.Contains(pattern)).ToList();

        if (Seasons.Count == seasons.Count) return;

        Seasons.Clear();

        seasons.ForEach(s => Seasons.Add(s));
    }

    private async Task Delete(object sender)
    {
        var button = sender as Button;
        var season = button?.DataContext as Season;

        ArgumentNullException.ThrowIfNull(season);
        
        _dbContext.Seasons.Remove(season);
        await _dbContext.SaveChangesAsync();

        Seasons.Remove(season);
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddSeasonViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(object sender)
    {
        var button = sender as Button;
        var season = button?.DataContext as Season;

        ArgumentNullException.ThrowIfNull(season);

        var model = App.ServiceProvider.GetService<AddSeasonViewModel>();

        model.Season = season.Adapt<UploadSeasonModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private void SeasonCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => SeasonCount = Seasons.Count;
}
