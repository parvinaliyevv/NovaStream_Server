namespace NovaStream.Admin.ViewModels.Tabs;

public class MovieGenreViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;

    private int _movieGenreCount;
    public int MovieGenreCount
    {
        get => _movieGenreCount;
        set { _movieGenreCount = value; RaisePropertyChanged(); }
    }

    public Movie Movie { get; set; }
    public ObservableCollection<MovieGenre> MovieGenres { get; set; }

    public RelayCommand SearchCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand OpenEditDialogHostCommand { get; set; }


    public MovieGenreViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task Initialize()
    {
        await Task.CompletedTask;

        MovieGenres = new ObservableCollection<MovieGenre>(_dbContext.MovieGenres.Include(mg => mg.Genre).Where(mg => mg.MovieName == Movie.Name));
        MovieGenreCount = MovieGenres.Count;

        MovieGenres.CollectionChanged += MovieGenreCountChanged;

        SearchCommand = new RelayCommand(sender => Search(sender));
        DeleteCommand = new RelayCommand(sender => Delete(sender));

        OpenAddDialogHostCommand = new RelayCommand(_ => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand(sender => OpenEditDialogHost(sender));
    }

    private async Task Search(object sender)
    {
        await Task.CompletedTask;

        var pattern = sender.ToString();

        var movieGenres = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.MovieGenres.Include(mg => mg.Genre).Where(mg => mg.MovieName == Movie.Name).ToList() :
            _dbContext.MovieGenres.Include(mg => mg.Genre).Where(mg => mg.MovieName == Movie.Name && mg.Genre.Name.Contains(pattern)).ToList();

        if (MovieGenres.Count == movieGenres.Count) return;

        MovieGenres.Clear();

        movieGenres.ForEach(mg => MovieGenres.Add(mg));
    }

    private async Task Delete(object sender)
    {
        var button = sender as Button;
        var movieGenre = button?.DataContext as MovieGenre;

        ArgumentNullException.ThrowIfNull(movieGenre);

        _dbContext.MovieGenres.Remove(movieGenre);
        await _dbContext.SaveChangesAsync();

        MovieGenres.Remove(movieGenre);
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddMovieGenreViewModel>();

        model.MovieGenre.Movie = Movie;
        model.Movies = new List<Movie> { Movie };

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(object sender)
    {
        var button = sender as Button;
        var movieGenre = button?.DataContext as MovieGenre;

        ArgumentNullException.ThrowIfNull(movieGenre);

        var model = App.ServiceProvider.GetService<AddMovieGenreViewModel>();

        model.MovieGenre.Movie = movieGenre.Movie;
        model.MovieGenre.Genre = movieGenre.Genre;

        await DialogHost.Show(model, "RootDialog");
    }

    private void MovieGenreCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => MovieGenreCount = MovieGenres.Count;
}
