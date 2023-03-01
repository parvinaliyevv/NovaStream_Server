namespace NovaStream.Admin.ViewModelContents.Concrete;

public class SerialActorViewModelContent : ViewModelContentBase
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

    private Serial _serial;
    public Serial Serial
    {
        get => _serial;
        set
        {
            _serial = value;

            ClearErrors(nameof(Serial));

            if (_serial is null) AddError(nameof(Serial), $"{nameof(Serial)} cannot be empty!");
        }
    }


    public override void Verify()
    {
        Serial = Serial;
        Actor = Actor;
    }
}
