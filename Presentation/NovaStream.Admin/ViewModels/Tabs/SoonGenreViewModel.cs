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

    private ObservableCollection<SoonGenre> _soonGenres;
    public ObservableCollection<SoonGenre> SoonGenres
    {
        get => _soonGenres;
        set { _soonGenres = value; RaisePropertyChanged(); }
    }
    
    public Soon Soon { get; set; }

    public RelayCommand SearchCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }


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
        RefreshCommand = new RelayCommand(_ => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(_ => OpenAddDialogHost());
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

        _ = MessageBoxService.Show($"Delete <{soonGenre.Genre.Name} from {soonGenre.SoonName}>...", MessageBoxType.Progress);

        _dbContext.SoonGenres.Remove(soonGenre);
        await _dbContext.SaveChangesAsync();

        SoonGenres.Remove(soonGenre);

        MessageBoxService.Close();
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddSoonGenreViewModel>();

        model.SoonGenre.Soon = Soon;
        model.Soons = new List<Soon> { Soon };

        var existsGenres = await _dbContext.SoonGenres.Include(sg => sg.Genre).Where(sg => sg.SoonName == Soon.Name).Select(sg => sg.Genre).ToListAsync();

        if (model.Genres.Count == existsGenres.Count)
        {
            await MessageBoxService.Show("Added all possible genres", MessageBoxType.Info);
            return;
        }

        foreach (var genre in existsGenres) model.Genres.Remove(genre);

        await DialogHost.Show(model, "RootDialog");
    }

    private void SoonGenreCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => SoonGenreCount = SoonGenres.Count;
}
