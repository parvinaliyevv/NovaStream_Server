namespace NovaStream.Admin.ViewModels.Tabs;

public class MovieActorViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;

    private int _movieActorCount;
    public int MovieActorCount
    {
        get => _movieActorCount;
        set { _movieActorCount = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<MovieActor> _movieActors;
    public ObservableCollection<MovieActor> MovieActors
    {
        get => _movieActors;
        set { _movieActors = value; RaisePropertyChanged(); }
    }

    public Movie Movie { get; set; }

    public RelayCommand SearchCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }


    public MovieActorViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task Initialize()
    {
        await Task.CompletedTask;

        MovieActors = new ObservableCollection<MovieActor>(_dbContext.MovieActors.Include(ma => ma.Actor).Where(ma => ma.MovieName == Movie.Name));
        MovieActorCount = MovieActors.Count;

        MovieActors.CollectionChanged += MovieActorCountChanged;

        SearchCommand = new RelayCommand(sender => Search(sender));
        DeleteCommand = new RelayCommand(sender => Delete(sender));
        RefreshCommand = new RelayCommand(_ => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(_ => OpenAddDialogHost());
    }

    private async Task Search(object sender)
    {
        await Task.CompletedTask;

        var pattern = sender.ToString();

        var movieActors = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.MovieActors.Include(ma => ma.Actor).Where(ma => ma.MovieName == Movie.Name).ToList() :
            _dbContext.MovieActors.Include(ma => ma.Actor).Where(ma => ma.MovieName == Movie.Name && ma.Actor.Name.Contains(pattern)).ToList();

        if (MovieActors.Count == movieActors.Count) return;

        MovieActors.Clear();

        movieActors.ForEach(ma => MovieActors.Add(ma));
    }

    private async Task Delete(object sender)
    {
        var button = sender as Button;
        var movieActor = button?.DataContext as MovieActor;

        ArgumentNullException.ThrowIfNull(movieActor);

        _ = MessageBoxService.Show($"Delete <{movieActor.Actor.Name} {movieActor.Actor.Surname} from {movieActor.MovieName}>...", MessageBoxType.Progress);

        _dbContext.MovieActors.Remove(movieActor);
        await _dbContext.SaveChangesAsync();

        MovieActors.Remove(movieActor);

        MessageBoxService.Close();
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddMovieActorViewModel>();

        model.MovieActor.Movie = Movie;
        model.Movies = new List<Movie> { Movie };

        var existsActors = await _dbContext.MovieActors.Include(ma => ma.Actor).Where(ma => ma.MovieName == Movie.Name).Select(ma => ma.Actor).ToListAsync();

        if (model.Actors.Count == existsActors.Count)
        {
            await MessageBoxService.Show("Added all possible actors", MessageBoxType.Info);
            return;
        }

        foreach (var actor in existsActors) model.Actors.Remove(actor);

        await DialogHost.Show(model, "RootDialog");
    }

    private void MovieActorCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => MovieActorCount = MovieActors.Count;
}
