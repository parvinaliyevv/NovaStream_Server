namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddMovieActorViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;

    public List<Movie> Movies { get; set; }
    public List<Actor> Actors { get; set; }
    public MovieActorViewModelContent MovieActor { get; set; }

    public bool ProcessStarted
    {
        get { return (bool)GetValue(ProcessStartedProperty); }
        set { SetValue(ProcessStartedProperty, value); }
    }
    public static readonly DependencyProperty ProcessStartedProperty =
        DependencyProperty.Register("ProcessStarted", typeof(bool), typeof(AddMovieActorViewModel));

    public RelayCommand SaveCommand { get; set; }


    public AddMovieActorViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        Actors = _dbContext.Actors.ToList();

        MovieActor = new MovieActorViewModelContent();

        SaveCommand = new RelayCommand(() => Save());
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        try
        {
            MovieActor.Verify();

            if (MovieActor.HasErrors) return;

            ProcessStarted = true;

            var dbMovieActor = _dbContext.MovieActors.Include(ma => ma.Actor)
                .FirstOrDefault(ma => ma.MovieName == MovieActor.Movie.Name && ma.Actor.Id == MovieActor.Actor.Id);

            if (dbMovieActor is not null) return;

            var movieActor = new MovieActor()
            {
                Movie = MovieActor.Movie,
                Actor = MovieActor.Actor
            };

            _dbContext.MovieActors.Add(movieActor);
            _dbContext.SaveChanges();

            App.ServiceProvider.GetService<MovieActorViewModel>()?.MovieActors.Add(movieActor);

            ProcessStarted = false;

            DialogHost.Close("RootDialog");

            await MessageBoxService.Show("Movie Actor saved succesfully!", MessageBoxType.Success);
        }
        catch
        {
            if (!InternetService.CheckInternet())
                await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error);

            else
                await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);

            ProcessStarted = false;
        }
    }
}
