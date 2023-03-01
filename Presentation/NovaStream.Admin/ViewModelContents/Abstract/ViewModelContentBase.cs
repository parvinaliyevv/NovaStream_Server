namespace NovaStream.Admin.ViewModelContents.Abstract;

public abstract class ViewModelContentBase : INotifyPropertyChanged, INotifyDataErrorInfo
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    private readonly Dictionary<string, List<string>> _propertyErrors = new();

    public bool HasErrors => _propertyErrors.Count > 0;


    public void AddError(string propertyName, string errorMessage)
    {
        if (!_propertyErrors.ContainsKey(propertyName))
        {
            _propertyErrors.Add(propertyName, new List<string>());
        }

        _propertyErrors[propertyName].Add(errorMessage);

        OnErrorsChanged(propertyName);
    }

    public void ClearErrors(string propertyName)
    {
        if (_propertyErrors.Remove(propertyName))
        {
            OnErrorsChanged(propertyName);
        }
    }

    public IEnumerable GetErrors(string? propertyName)
        => _propertyErrors.GetValueOrDefault(propertyName, null);

    private void OnErrorsChanged(string? propertyName) 
        => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) 
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public abstract void Verify();
}
