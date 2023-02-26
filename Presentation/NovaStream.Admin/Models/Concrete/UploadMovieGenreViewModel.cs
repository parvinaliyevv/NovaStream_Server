namespace NovaStream.Admin.Models.Concrete;

public class UploadMovieGenreViewModel : ModelBase
{
	private Genre _genre;
	public Genre Genre
	{
		get => _genre;
		set 
		{
			_genre = value;

			ClearErrors(nameof(Genre));

            if (_movie is null) AddError(nameof(Movie), "Movie cannot be empty!");
        }
	}

    private Movie _movie;
    public Movie Movie
    {
        get => _movie;
        set
        {
            _movie = value;

            ClearErrors(nameof(Movie));

            if (_movie is null) AddError(nameof(Movie), "Movie cannot be empty!");
        }
    }


    public override void Verify()
    {
        Movie = Movie;
        Genre = Genre;
    }
}
