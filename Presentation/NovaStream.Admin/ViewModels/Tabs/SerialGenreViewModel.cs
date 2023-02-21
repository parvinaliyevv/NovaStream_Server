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

    public Serial Serial { get; set; }
    public ObservableCollection<SerialGenre> SerialGenres { get; set; }

    public RelayCommand SearchCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand OpenEditDialogHostCommand { get; set; }


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

        OpenAddDialogHostCommand = new RelayCommand(_ => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand(sender => OpenEditDialogHost(sender));
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

        _dbContext.SerialGenres.Remove(serialGenre);
        await _dbContext.SaveChangesAsync();

        SerialGenres.Remove(serialGenre);
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddSerialGenreViewModel>();

        model.SerialGenre.Serial = Serial;
        model.Serials = new List<Serial> { Serial };

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(object sender)
    {
        var button = sender as Button;
        var serialGenre = button?.DataContext as SerialGenre;

        ArgumentNullException.ThrowIfNull(serialGenre);

        var model = App.ServiceProvider.GetService<AddSerialGenreViewModel>();

        model.SerialGenre.Serial = serialGenre.Serial;
        model.SerialGenre.Genre = serialGenre.Genre;

        await DialogHost.Show(model, "RootDialog");
    }

    private void SerialGenreCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => SerialGenreCount = SerialGenres.Count;
}
