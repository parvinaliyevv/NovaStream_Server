namespace NovaStream.Admin.Services;

public class BlobStorageUploadProgress : IProgress<long> ,INotifyPropertyChanged
{
    public long Length { get; set; }

    private int _progress;
    public int Progress
    {
        get { return _progress; }
        set { _progress = value; OnPropertyChanged(); }
    }


    public BlobStorageUploadProgress(long length)
    {
        Length = length;
    }


    public void Report(long value)
    {
        try
        {
            Progress = (int)(value * 100 / Length);
        }
        catch { }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChangedEventHandler? handler = PropertyChanged;
        if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
}
