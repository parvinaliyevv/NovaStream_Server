namespace NovaStream.Admin.ViewModels;

public class SerialGenreViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;

    private int _serialGenreCount;
    public int SerialGenreCount
    {
        get => _serialGenreCount;
        set { _serialGenreCount = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<SerialGenre> _serialGenres;
    public ObservableCollection<SerialGenre> SerialGenres
    {
        get => _serialGenres;
        set { _serialGenres = value; RaisePropertyChanged(); }
    }

    public Serial Serial { get; set; }

    public RelayCommand<string> SearchCommand { get; set; }
    public RelayCommand<SerialGenre> DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }


    public SerialGenreViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        SearchCommand = new RelayCommand<string>(pattern => Search(pattern));
        DeleteCommand = new RelayCommand<SerialGenre>(serialGenre => Delete(serialGenre));
        RefreshCommand = new RelayCommand(() => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
    }


    public async Task Initialize()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }
        
        _ = MessageBoxService.Show($"Loading serial genres...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            SerialGenres = new ObservableCollection<SerialGenre>(_dbContext.SerialGenres.Include(sg => sg.Genre).Where(sg => sg.SerialName == Serial.Name));
            SerialGenreCount = SerialGenres.Count;

            SerialGenres.CollectionChanged += SerialGenreCountChanged;

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
            _dbContext.SerialGenres.Include(sg => sg.Genre).Where(sg => sg.SerialName == Serial.Name).ToList() :
            _dbContext.SerialGenres.Include(sg => sg.Genre).Where(sg => sg.SerialName == Serial.Name && sg.Genre.Name.Contains(pattern)).ToList();

            if (SerialGenres.Count == soonGenres.Count) return;

            SerialGenres.Clear();

            soonGenres.ForEach(sg => SerialGenres.Add(sg));
        }
        catch
        {
            await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);
        }
    }

    private async Task Delete(SerialGenre serialGenre)
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(serialGenre);

        _ = MessageBoxService.Show($"Delete <{serialGenre.Genre.Name} from {serialGenre.SerialName}>...", MessageBoxType.Progress);

        await Task.CompletedTask;

        try
        {
            _dbContext.SerialGenres.Remove(serialGenre);
            await _dbContext.SaveChangesAsync();

            SerialGenres.Remove(serialGenre);

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

        var model = App.ServiceProvider.GetService<AddSerialGenreViewModel>();

        model.SerialGenre.Serial = Serial;
        model.Serials = new List<Serial> { Serial };

        var existsGenres = await _dbContext.SerialGenres.Include(sg => sg.Genre).Where(sg => sg.SerialName == Serial.Name).Select(sg => sg.Genre).ToListAsync();

        if (model.Genres.Count == existsGenres.Count)
        {
            await MessageBoxService.Show("Added all possible genres", MessageBoxType.Info);
            return;
        }

        foreach (var genre in existsGenres) model.Genres.Remove(genre);

        await DialogHost.Show(model, "RootDialog");
    }

    private void SerialGenreCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => SerialGenreCount = SerialGenres.Count;
}
