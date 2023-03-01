namespace NovaStream.Admin.ViewModels;

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

    public RelayCommand<string> SearchCommand { get; set; }
    public RelayCommand<MovieActor> DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }


    public MovieActorViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        SearchCommand = new RelayCommand<string>(pattern => Search(pattern));
        DeleteCommand = new RelayCommand<MovieActor>(movieActor => Delete(movieActor));
        RefreshCommand = new RelayCommand(() => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
    }


    public async Task Initialize()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }
        
        _ = MessageBoxService.Show($"Loading movie actors...", MessageBoxType.Progress);

        await Task.Delay(1000);
        
        try
        {
            MovieActors = new ObservableCollection<MovieActor>(_dbContext.MovieActors.Include(ma => ma.Actor).Where(ma => ma.MovieName == Movie.Name));
            MovieActorCount = MovieActors.Count;

            MovieActors.CollectionChanged += MovieActorCountChanged;

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
            var movieActors = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.MovieActors.Include(ma => ma.Actor).Where(ma => ma.MovieName == Movie.Name).ToList() :
            _dbContext.MovieActors.Include(ma => ma.Actor).Where(ma => ma.MovieName == Movie.Name && ma.Actor.Name.Contains(pattern)).ToList();

            if (MovieActors.Count == movieActors.Count) return;

            MovieActors.Clear();

            movieActors.ForEach(ma => MovieActors.Add(ma));
        }
        catch
        {
            await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);
        }
    }

    private async Task Delete(MovieActor movieActor)
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(movieActor);

        _ = MessageBoxService.Show($"Delete <{movieActor.Actor.Name} {movieActor.Actor.Surname} from {movieActor.MovieName}>...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            _dbContext.MovieActors.Remove(movieActor);
            await _dbContext.SaveChangesAsync();

            MovieActors.Remove(movieActor);

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
