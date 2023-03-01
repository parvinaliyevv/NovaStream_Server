namespace NovaStream.Admin.ViewModels;

public class MovieGenreViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;

    private int _movieGenreCount;
    public int MovieGenreCount
    {
        get => _movieGenreCount;
        set { _movieGenreCount = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<MovieGenre> _movieGenres;
    public ObservableCollection<MovieGenre> MovieGenres
    {
        get => _movieGenres;
        set { _movieGenres = value; RaisePropertyChanged(); }
    }

    public Movie Movie { get; set; }

    public RelayCommand<string> SearchCommand { get; set; }
    public RelayCommand<MovieGenre> DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }


    public MovieGenreViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        SearchCommand = new RelayCommand<string>(pattern => Search(pattern));
        DeleteCommand = new RelayCommand<MovieGenre>(movieGenre => Delete(movieGenre));
        RefreshCommand = new RelayCommand(() => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
    }


    public async Task Initialize()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        _ = MessageBoxService.Show($"Loading movie genres...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            MovieGenres = new ObservableCollection<MovieGenre>(_dbContext.MovieGenres.Include(mg => mg.Genre).Where(mg => mg.MovieName == Movie.Name));
            MovieGenreCount = MovieGenres.Count;

            MovieGenres.CollectionChanged += MovieGenreCountChanged;

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
            var movieGenres = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.MovieGenres.Include(mg => mg.Genre).Where(mg => mg.MovieName == Movie.Name).ToList() :
            _dbContext.MovieGenres.Include(mg => mg.Genre).Where(mg => mg.MovieName == Movie.Name && mg.Genre.Name.Contains(pattern)).ToList();

            if (MovieGenres.Count == movieGenres.Count) return;

            MovieGenres.Clear();

            movieGenres.ForEach(mg => MovieGenres.Add(mg));
        }
        catch
        {
            await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);
        }
    }

    private async Task Delete(MovieGenre movieGenre)
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(movieGenre);

        _ = MessageBoxService.Show($"Delete <{movieGenre.Genre.Name} from {movieGenre.MovieName}>...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            _dbContext.MovieGenres.Remove(movieGenre);
            await _dbContext.SaveChangesAsync();

            MovieGenres.Remove(movieGenre);

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

        var model = App.ServiceProvider.GetService<AddMovieGenreViewModel>();

        model.MovieGenre.Movie = Movie;
        model.Movies = new List<Movie> { Movie };

        var existsGenres = await _dbContext.MovieGenres.Include(mg => mg.Genre).Where(mg => mg.MovieName == Movie.Name).Select(mg => mg.Genre).ToListAsync();

        if (model.Genres.Count == existsGenres.Count)
        {
            await MessageBoxService.Show("Added all possible genres", MessageBoxType.Info);
            return;
        }

        foreach (var genre in existsGenres) model.Genres.Remove(genre);

        await DialogHost.Show(model, "RootDialog");
    }

    private void MovieGenreCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => MovieGenreCount = MovieGenres.Count;
}
