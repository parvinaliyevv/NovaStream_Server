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

    private ObservableCollection<MovieGenre> _movieGenres;
    public ObservableCollection<MovieGenre> MovieGenres
    {
        get => _movieGenres;
        set { _movieGenres = value; RaisePropertyChanged(); }
    }
   
    public Movie Movie { get; set; }

    public RelayCommand SearchCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }


    public RelayCommand OpenAddDialogHostCommand { get; set; }


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
        RefreshCommand = new RelayCommand(_ => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(_ => OpenAddDialogHost());
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

        _ = MessageBoxService.Show($"Delete <{movieGenre.Genre.Name} from {movieGenre.MovieName}>...", MessageBoxType.Progress);

        _dbContext.MovieGenres.Remove(movieGenre);
        await _dbContext.SaveChangesAsync();

        MovieGenres.Remove(movieGenre);

        MessageBoxService.Close();
    }

    private async Task OpenAddDialogHost()
    {
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
