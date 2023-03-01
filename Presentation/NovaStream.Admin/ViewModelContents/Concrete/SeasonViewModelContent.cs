namespace NovaStream.Admin.ViewModelContents.Concrete;

public class SeasonViewModelContent : ViewModelContentBase
{
    private int _number;
    public int Number
    {
        get => _number;
        set
        {
            _number = value;

            OnPropertyChanged();

            ClearErrors(nameof(Number));

            if (Number <= 0) AddError(nameof(Number), $"{nameof(Number)} cannot be negative or zero!");
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

            if (_serial is null) AddError(nameof(Serial), $"{nameof(Serial)} cannot be empty!");
        }
    }


    public override void Verify()
    {
        Serial = Serial;
    }
}
