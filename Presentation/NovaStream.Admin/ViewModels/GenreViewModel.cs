namespace NovaStream.Admin.ViewModels;

public class GenreViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;

    private int _genreCount;
    public int GenreCount
    {
        get => _genreCount;
        set { _genreCount = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<Genre> _genres;
    public ObservableCollection<Genre> Genres
    {
        get => _genres;
        set { _genres = value; RaisePropertyChanged(); }
    }

    public RelayCommand SearchCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand OpenEditDialogHostCommand { get; set; }


    public GenreViewModel(AppDbContext dbContext, IStorageManager storageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;

        Initialize();
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        Genres = new ObservableCollection<Genre>(_dbContext.Genres);
        GenreCount = Genres.Count;

        Genres.CollectionChanged += GenreCountChanged;

        SearchCommand = new RelayCommand(sender => Search(sender));
        DeleteCommand = new RelayCommand(sender => Delete(sender));
        RefreshCommand = new RelayCommand(_ => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(_ => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand(sender => OpenEditDialogHost(sender));
    }

    private async Task Search(object sender)
    {
        await Task.CompletedTask;

        var pattern = sender.ToString();

        var genres = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.Genres.ToList() : 
            _dbContext.Genres.Where(g => g.Name.Contains(pattern)).ToList();

        if (Genres.Count == genres.Count) return;

        Genres.Clear();

        genres.ForEach(g => Genres.Add(g));
    }

    private async Task Delete(object sender)
    {
        var button = sender as Button;
        var genre = button?.DataContext as Genre;

        ArgumentNullException.ThrowIfNull(genre);

        _ = MessageBoxService.Show($"Delete <{genre.Name}>...", MessageBoxType.Progress);
        
        await _storageManager.DeleteFileAsync(genre.ImageUrl);

        _dbContext.Genres.Remove(genre);
        await _dbContext.SaveChangesAsync();

        Genres.Remove(genre);

        MessageBoxService.Close();
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddGenreViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(object sender)
    {
        var button = sender as Button;
        var genre = button?.DataContext as Genre;

        ArgumentNullException.ThrowIfNull(genre);

        var model = App.ServiceProvider.GetService<AddGenreViewModel>();

        model.Genre = genre.Adapt<UploadGenreModel>();
        model.IsEdit = true;

        await DialogHost.Show(model, "RootDialog");
    }

    private void GenreCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => GenreCount = Genres.Count;
}
