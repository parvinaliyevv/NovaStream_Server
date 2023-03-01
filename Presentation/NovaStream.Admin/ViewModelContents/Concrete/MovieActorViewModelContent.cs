namespace NovaStream.Admin.ViewModelContents.Concrete;

public class MovieActorViewModelContent : ViewModelContentBase
{
    private Actor _actor;
    public Actor Actor
    {
        get => _actor;
        set
        {
            _actor = value;

            ClearErrors(nameof(Actor));

            if (_actor is null) AddError(nameof(Actor), $"{nameof(Actor)} cannot be empty!");
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
        Actor = Actor;
    }
}
