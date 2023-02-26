namespace NovaStream.Admin.Models.Concrete;

public class UploadSoonGenreViewModel : ModelBase
{
    private Genre _genre;
    public Genre Genre
    {
        get => _genre;
        set
        {
            _genre = value;

            ClearErrors(nameof(Genre));

            if (_soon is null) AddError(nameof(Soon), "Movie cannot be empty!");
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

            if (_soon is null) AddError(nameof(Soon), "Soon cannot be empty!");
        }
    }


    public override void Verify()
    {
        Soon = Soon;
        Genre = Genre;
    }
}
