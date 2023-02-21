namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddMovieGenreViewModel
{
    private readonly AppDbContext _dbContext;

    public List<Movie> Movies { get; set; }
    public List<Genre> Genres { get; set; }
    public UploadMovieGenreViewModel MovieGenre { get; set; }

    public RelayCommand SaveCommand { get; set; }


    public AddMovieGenreViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        Genres = _dbContext.Genres.ToList();
        MovieGenre = new UploadMovieGenreViewModel();
        
        SaveCommand = new RelayCommand(_ => Save());
    }


    private void Save()
    {
        MovieGenre.Verify();

        if (MovieGenre.HasErrors) return;

        var dbMovieGenre = _dbContext.MovieGenres.Include(mg => mg.Genre)
            .FirstOrDefault(mg => mg.MovieName == MovieGenre.Movie.Name && mg.Genre.Id == MovieGenre.Genre.Id);

        if (dbMovieGenre is not null) return;

        var movieGenre = new MovieGenre()
        {
            Movie = MovieGenre.Movie,
            Genre = MovieGenre.Genre
        };

        _dbContext.MovieGenres.Add(movieGenre);
        _dbContext.SaveChanges();

        App.ServiceProvider.GetService<MovieGenreViewModel>().MovieGenres.Add(movieGenre);
    }
}
