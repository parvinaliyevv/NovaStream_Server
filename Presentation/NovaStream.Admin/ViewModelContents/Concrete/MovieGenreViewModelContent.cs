namespace NovaStream.Admin.ViewModelContents.Concrete;

public class MovieGenreViewModelContent : ViewModelContentBase
{
	private Genre _genre;
	public Genre Genre
	{
		get => _genre;
		set 
		{
			_genre = value;

			ClearErrors(nameof(Genre));

            if (_genre is null) AddError(nameof(Genre), $"{nameof(Genre)} cannot be empty!");
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

            if (_movie is null) AddError(nameof(Movie), $"{nameof(Movie)} cannot be empty!");
        }
    }


    public override void Verify()
    {
        Movie = Movie;
        Genre = Genre;
    }
}
