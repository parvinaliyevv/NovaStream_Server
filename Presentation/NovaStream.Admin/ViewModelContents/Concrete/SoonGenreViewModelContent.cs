namespace NovaStream.Admin.ViewModelContents.Concrete;

public class SoonGenreViewModelContent : ViewModelContentBase
{
    private Genre _genre;
    public Genre Genre
    {
        get => _genre;
        set
        {
            _genre = value;

            ClearErrors(nameof(Genre));

            if (_genre is null) AddError(nameof(Genre), $"{Genre} cannot be empty!");
        }
    }

    private Soon _soon;
    public Soon Soon
    {
        get => _soon;
        set
        {
            _soon = value;

            ClearErrors(nameof(Soon));

            if (_soon is null) AddError(nameof(Soon), $"{Soon} cannot be empty!");
        }
    }


    public override void Verify()
    {
        Soon = Soon;
        Genre = Genre;
    }
}
