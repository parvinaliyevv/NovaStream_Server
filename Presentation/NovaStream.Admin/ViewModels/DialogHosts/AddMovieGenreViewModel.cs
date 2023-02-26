﻿namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddMovieGenreViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;

    public List<Movie> Movies { get; set; }
    public List<Genre> Genres { get; set; }
    public UploadMovieGenreViewModel MovieGenre { get; set; }

    public bool ProcessStarted
    {
        get { return (bool)GetValue(ProcessStartedProperty); }
        set { SetValue(ProcessStartedProperty, value); }
    }
    public static readonly DependencyProperty ProcessStartedProperty =
        DependencyProperty.Register("ProcessStarted", typeof(bool), typeof(AddMovieGenreViewModel));

    public RelayCommand SaveCommand { get; set; }


    public AddMovieGenreViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        Genres = _dbContext.Genres.ToList();
        MovieGenre = new UploadMovieGenreViewModel();
        
        SaveCommand = new RelayCommand(_ => Save());
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        try
        {
            MovieGenre.Verify();

            if (MovieGenre.HasErrors) return;

            ProcessStarted = true;

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

            App.ServiceProvider.GetService<MovieGenreViewModel>()?.MovieGenres.Add(movieGenre);

            ProcessStarted = false;

            DialogHost.Close("RootDialog");

            await MessageBoxService.Show("Movie Genre saved succesfully!", MessageBoxType.Success);
        }
        catch (Exception ex)
        {
            await MessageBoxService.Show(ex.Message, MessageBoxType.Error);

            ProcessStarted = false;
        }
    }
}