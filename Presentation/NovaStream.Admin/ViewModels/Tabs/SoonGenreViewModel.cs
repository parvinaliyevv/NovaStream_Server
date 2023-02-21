namespace NovaStream.Admin.ViewModels.Tabs;

public class SoonGenreViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;

    private int _soonGenreCount;
    public int SoonGenreCount
    {
        get => _soonGenreCount;
        set { _soonGenreCount = value; RaisePropertyChanged(); }
    }

    public Soon Soon { get; set; }
    public ObservableCollection<SoonGenre> SoonGenres { get; set; }

    public RelayCommand SearchCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand OpenEditDialogHostCommand { get; set; }


    public SoonGenreViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task Initialize()
    {
        await Task.CompletedTask;

        SoonGenres = new ObservableCollection<SoonGenre>(_dbContext.SoonGenres.Include(sg => sg.Genre).Where(sg => sg.SoonName == Soon.Name));
        SoonGenreCount = SoonGenres.Count;

        SoonGenres.CollectionChanged += SoonGenreCountChanged;

        SearchCommand = new RelayCommand(sender => Search(sender));
        DeleteCommand = new RelayCommand(sender => Delete(sender));

        OpenAddDialogHostCommand = new RelayCommand(_ => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand(sender => OpenEditDialogHost(sender));
    }

    private async Task Search(object sender)
    {
        await Task.CompletedTask;

        var pattern = sender.ToString();

        var soonGenres = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.SoonGenres.Include(sg => sg.Genre).Where(sg => sg.SoonName == Soon.Name).ToList() : 
            _dbContext.SoonGenres.Include(sg => sg.Genre).Where(sg => sg.SoonName == Soon.Name && sg.Genre.Name.Contains(pattern)).ToList();

        if (SoonGenres.Count == soonGenres.Count) return;

        SoonGenres.Clear();

        soonGenres.ForEach(sg => SoonGenres.Add(sg));
    }

    private async Task Delete(object sender)
    {
        var button = sender as Button;
        var soonGenre = button?.DataContext as SoonGenre;

        ArgumentNullException.ThrowIfNull(soonGenre);

        _dbContext.SoonGenres.Remove(soonGenre);
        await _dbContext.SaveChangesAsync();

        SoonGenres.Remove(soonGenre);
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddSoonGenreViewModel>();

        model.SoonGenre.Soon = Soon;
        model.Soons = new List<Soon> { Soon };

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(object sender)
    {
        var button = sender as Button;
        var soonGenre = button?.DataContext as SoonGenre;

        ArgumentNullException.ThrowIfNull(soonGenre);

        var model = App.ServiceProvider.GetService<AddSoonGenreViewModel>();

        model.SoonGenre.Soon = soonGenre.Soon;
        model.SoonGenre.Genre = soonGenre.Genre;

        await DialogHost.Show(model, "RootDialog");
    }

    private void SoonGenreCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => SoonGenreCount = SoonGenres.Count;
}
