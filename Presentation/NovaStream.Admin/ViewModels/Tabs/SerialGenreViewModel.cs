namespace NovaStream.Admin.ViewModels.Tabs;

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

    public RelayCommand SearchCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }


    public SerialGenreViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task Initialize()
    {
        await Task.CompletedTask;

        SerialGenres = new ObservableCollection<SerialGenre>(_dbContext.SerialGenres.Include(sg => sg.Genre).Where(sg => sg.SerialName == Serial.Name));
        SerialGenreCount = SerialGenres.Count;

        SerialGenres.CollectionChanged += SerialGenreCountChanged;

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
            _dbContext.SerialGenres.Include(sg => sg.Genre).Where(sg => sg.SerialName == Serial.Name).ToList() :
            _dbContext.SerialGenres.Include(sg => sg.Genre).Where(sg => sg.SerialName == Serial.Name && sg.Genre.Name.Contains(pattern)).ToList();

        if (SerialGenres.Count == soonGenres.Count) return;

        SerialGenres.Clear();

        soonGenres.ForEach(sg => SerialGenres.Add(sg));
    }

    private async Task Delete(object sender)
    {
        var button = sender as Button;
        var serialGenre = button?.DataContext as SerialGenre;

        ArgumentNullException.ThrowIfNull(serialGenre);

        _ = MessageBoxService.Show($"Delete <{serialGenre.Genre.Name} from {serialGenre.SerialName}>...", MessageBoxType.Progress);
        
        _dbContext.SerialGenres.Remove(serialGenre);
        await _dbContext.SaveChangesAsync();

        SerialGenres.Remove(serialGenre);

        MessageBoxService.Close();
    }

    private async Task OpenAddDialogHost()
    {
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
