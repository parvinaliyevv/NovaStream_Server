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

    public RelayCommand<string> SearchCommand { get; set; }
    public RelayCommand<Genre> DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand<Genre> OpenEditDialogHostCommand { get; set; }


    public GenreViewModel(AppDbContext dbContext, IStorageManager storageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;

        Initialize();

        SearchCommand = new RelayCommand<string>(pattern => Search(pattern));
        DeleteCommand = new RelayCommand<Genre>(genre => Delete(genre));
        RefreshCommand = new RelayCommand(() => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand<Genre>(genre => OpenEditDialogHost(genre));
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }
        
        _ = MessageBoxService.Show($"Loading genres...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            Genres = new ObservableCollection<Genre>(_dbContext.Genres);
            GenreCount = Genres.Count;

            Genres.CollectionChanged += GenreCountChanged;

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
            var genres = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.Genres.ToList() :
            _dbContext.Genres.Where(g => g.Name.Contains(pattern)).ToList();

            if (Genres.Count == genres.Count) return;

            Genres.Clear();

            genres.ForEach(g => Genres.Add(g));
        }
        catch
        {
            await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);
        }
    }

    private async Task Delete(Genre genre)
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(genre);

        _ = MessageBoxService.Show($"Delete <{genre.Name}>...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            var imageUrl = genre.ImageUrl;

            _dbContext.Genres.Remove(genre);
            await _dbContext.SaveChangesAsync();

            await _storageManager.DeleteFileAsync(imageUrl);

            Genres.Remove(genre);

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

        var model = App.ServiceProvider.GetService<AddGenreViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(Genre genre)
    {
        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(genre);

        var model = App.ServiceProvider.GetService<AddGenreViewModel>();

        model.Genre = genre.Adapt<GenreViewModelContent>();
        model.IsEdit = true;

        await DialogHost.Show(model, "RootDialog");
    }

    private void GenreCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => GenreCount = Genres.Count;
}
