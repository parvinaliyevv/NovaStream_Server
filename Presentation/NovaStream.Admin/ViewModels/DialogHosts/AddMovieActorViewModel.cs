namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddMovieActorViewModel
{
    private readonly AppDbContext _dbContext;

    public List<Movie> Movies { get; set; }
    public List<Actor> Actors { get; set; }
    public UploadMovieActorViewModel MovieActor { get; set; }

    public RelayCommand SaveCommand { get; set; }


    public AddMovieActorViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        Actors = _dbContext.Actors.ToList();
        MovieActor = new UploadMovieActorViewModel();

        SaveCommand = new RelayCommand(_ => Save());
    }


    private void Save()
    {
        MovieActor.Verify();

        if (MovieActor.HasErrors) return;

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

        App.ServiceProvider.GetService<MovieActorViewModel>().MovieActors.Add(movieActor);
    }
}
