namespace NovaStream.Admin.Models.Concrete;

public class UploadSerialGenreViewModel : ModelBase
{
    private Genre _genre;
    public Genre Genre
    {
        get => _genre;
        set
        {
            _genre = value;

            ClearErrors(nameof(Genre));

            if (_serial is null) AddError(nameof(Serial), "Movie cannot be empty!");
        }
    }

    private Serial _serial;
    public Serial Serial
    {
        get => _serial;
        set
        {
            _serial = value;

            ClearErrors(nameof(Serial));

            if (_serial is null) AddError(nameof(Serial), "Serial cannot be empty!");
        }
    }


    public override void Verify()
    {
        Serial = Serial;
        Genre = Genre;
    }
}
