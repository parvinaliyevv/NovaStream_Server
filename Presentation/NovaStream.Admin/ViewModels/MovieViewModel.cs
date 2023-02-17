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
    
    public ObservableCollection<Movie> Movies { get; set; }

    public RelayCommand SearchCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand OpenEditDialogHostCommand { get; set; }


    public MovieViewModel(AppDbContext dbContext, IStorageManager storageManager, IAWSStorageManager awsStorageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;
        _awsStorageManager = awsStorageManager;

        Initialize();
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        Movies = new ObservableCollection<Movie>(_dbContext.Movies.Include(s => s.Producer));
        MovieCount = Movies.Count;

        Movies.CollectionChanged += MovieCountChanged;

        SearchCommand = new RelayCommand(sender => Search(sender));
        DeleteCommand = new RelayCommand(sender => Delete(sender));

        OpenAddDialogHostCommand = new RelayCommand(_ => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand(sender => OpenEditDialogHost(sender));
    }

    private async Task Search(object sender)
    {
        await Task.CompletedTask;

        var pattern = sender.ToString();

        var movies = string.IsNullOrWhiteSpace(pattern)
            ? _dbContext.Movies.ToList() : _dbContext.Movies.Where(m => m.Name.Contains(pattern)).ToList();

        if (Movies.Count == movies.Count) return;

        Movies.Clear();

        movies.ForEach(m => Movies.Add(m));
    }

    private async Task Delete(object sender)
    {
        var button = sender as Button;
        var movie = button?.DataContext as Movie;

        ArgumentNullException.ThrowIfNull(movie);

        await _awsStorageManager.DeleteFileAsync(movie.VideoUrl);
        await _storageManager.DeleteFileAsync(movie.VideoImageUrl);
        await _storageManager.DeleteFileAsync(movie.TrailerUrl);
        await _storageManager.DeleteFileAsync(movie.ImageUrl);
        await _storageManager.DeleteFileAsync(movie.SearchImageUrl);

        _dbContext.Movies.Remove(movie);
        await _dbContext.SaveChangesAsync();

        Movies.Remove(movie);
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddMovieViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(object sender)
    {
        var button = sender as Button;
        var movie = button?.DataContext as Movie;

        ArgumentNullException.ThrowIfNull(movie);

        var model = App.ServiceProvider.GetService<AddMovieViewModel>();
        
        model.Movie = movie.Adapt<UploadMovieModel>();
        model.Movie.Producer = movie.Producer;

        await DialogHost.Show(model, "RootDialog");
    }

    private void MovieCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => MovieCount = Movies.Count;
}
