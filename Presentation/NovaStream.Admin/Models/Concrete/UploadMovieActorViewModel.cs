namespace NovaStream.Admin.Models.Concrete;

public class UploadMovieActorViewModel : ModelBase
{
    private Actor _actor;
    public Actor Actor
    {
        get => _actor;
        set
        {
            _actor = value;

            ClearErrors(nameof(Actor));

            if (_actor is null) AddError(nameof(Actor), "Actor cannot be empty!");
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
        Actor = Actor;
    }
}
