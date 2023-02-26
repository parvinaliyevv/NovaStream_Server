namespace NovaStream.Admin.Models.Concrete;

public class UploadSeasonModel : ModelBase
{
    private int _number;
    public int Number
    {
        get => _number;
        set
        {
            _number = value;

            OnPropertyChanged();
        }
    }

    private Serial _serial;
    public Serial Serial
    {
        get => _serial;
        set
        {
            _serial = value;

            OnPropertyChanged();

            ClearErrors(nameof(Serial));

            if (_serial is null) AddError(nameof(Serial), "Serial cannot be empty!");
        }
    }


    public override void Verify()
    {
        Serial = Serial;
    }
}
