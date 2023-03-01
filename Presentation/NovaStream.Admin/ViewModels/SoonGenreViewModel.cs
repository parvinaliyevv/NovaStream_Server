namespace NovaStream.Admin.ViewModels;

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

    public RelayCommand<string> SearchCommand { get; set; }
    public RelayCommand<SoonGenre> DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }


    public SoonGenreViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        SearchCommand = new RelayCommand<string>(pattern => Search(pattern));
        DeleteCommand = new RelayCommand<SoonGenre>(soonGenre => Delete(soonGenre));
        RefreshCommand = new RelayCommand(() => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
    }


    public async Task Initialize()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        _ = MessageBoxService.Show($"Loading soon genres...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            SoonGenres = new ObservableCollection<SoonGenre>(_dbContext.SoonGenres.Include(sg => sg.Genre).Where(sg => sg.SoonName == Soon.Name));
            SoonGenreCount = SoonGenres.Count;

            SoonGenres.CollectionChanged += SoonGenreCountChanged;

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
            var soonGenres = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.SoonGenres.Include(sg => sg.Genre).Where(sg => sg.SoonName == Soon.Name).ToList() :
            _dbContext.SoonGenres.Include(sg => sg.Genre).Where(sg => sg.SoonName == Soon.Name && sg.Genre.Name.Contains(pattern)).ToList();

            if (SoonGenres.Count == soonGenres.Count) return;

            SoonGenres.Clear();

            soonGenres.ForEach(sg => SoonGenres.Add(sg));
        }
        catch
        {
            await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);
        }
    }

    private async Task Delete(SoonGenre soonGenre)
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(soonGenre);

        _ = MessageBoxService.Show($"Delete <{soonGenre.Genre.Name} from {soonGenre.SoonName}>...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            _dbContext.SoonGenres.Remove(soonGenre);
            await _dbContext.SaveChangesAsync();

            SoonGenres.Remove(soonGenre);

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
