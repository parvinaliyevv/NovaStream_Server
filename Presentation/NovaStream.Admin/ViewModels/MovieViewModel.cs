namespace NovaStream.Admin.ViewModels;

public class MovieViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;
    private readonly IAWSStorageManager _awsStorageManager;

    private int _movieCount;
    public int MovieCount
    {
        get => _movieCount;
        set { _movieCount = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<Movie> _movies;
    public ObservableCollection<Movie> Movies
    {
        get => _movies;
        set { _movies = value; RaisePropertyChanged(); }
    }

    public RelayCommand<string> SearchCommand { get; set; }
    public RelayCommand<Movie> DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand<Movie> OpenEditDialogHostCommand { get; set; }


    public MovieViewModel(AppDbContext dbContext, IStorageManager storageManager, IAWSStorageManager awsStorageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;
        _awsStorageManager = awsStorageManager;

        Initialize();

        SearchCommand = new RelayCommand<string>(pattern => Search(pattern));
        DeleteCommand = new RelayCommand<Movie>(movie => Delete(movie));
        RefreshCommand = new RelayCommand(() => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand<Movie>(movie => OpenEditDialogHost(movie));
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        try
        {
            _ = MessageBoxService.Show($"Loading movies...", MessageBoxType.Progress);

            await Task.Delay(1000);
        }
        catch { }

        try
        {
            Movies = new ObservableCollection<Movie>(_dbContext.Movies.Include(s => s.Director));
            MovieCount = Movies.Count;

            Movies.CollectionChanged += MovieCountChanged;

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
            var movies = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.Movies.Include(m => m.Director).ToList() :
            _dbContext.Movies.Include(m => m.Director).Where(m => m.Name.Contains(pattern)).ToList();

            if (Movies.Count == movies.Count) return;

            Movies.Clear();

            movies.ForEach(m => Movies.Add(m));
        }
        catch
        {
            await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);
        }
    }

    private async Task Delete(Movie movie)
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(movie);

        _ = MessageBoxService.Show($"Delete <{movie.Name}>...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            var videoUrl = movie.VideoUrl;
            var videoImageUrl = movie.VideoImageUrl;
            var trailerUrl = movie.TrailerUrl;
            var imageUrl = movie.ImageUrl;
            var searchImageUrl = movie.SearchImageUrl;

            _dbContext.Movies.Remove(movie);
            await _dbContext.SaveChangesAsync();

            Movies.Remove(movie);

            await _awsStorageManager.DeleteFileAsync(videoUrl);
            await _storageManager.DeleteFileAsync(videoImageUrl);
            await _storageManager.DeleteFileAsync(trailerUrl);
            await _storageManager.DeleteFileAsync(imageUrl);
            await _storageManager.DeleteFileAsync(searchImageUrl);

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

        var model = App.ServiceProvider.GetService<AddMovieViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(Movie movie)
    {
        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(movie);

        var model = App.ServiceProvider.GetService<AddMovieViewModel>();

        model.Movie = movie.Adapt<MovieViewModelContent>();
        model.Movie.Director = movie.Director;
        model.IsEdit = true;

        await DialogHost.Show(model, "RootDialog");
    }

    private void MovieCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => MovieCount = Movies.Count;
}
