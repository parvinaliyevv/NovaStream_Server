namespace NovaStream.Admin.ViewModelContents.Concrete;

public class EpisodeViewModelContent : ViewModelContentBase
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

    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;

            OnPropertyChanged();

            ClearErrors(nameof(Name));

            if (string.IsNullOrWhiteSpace(_name)) AddError(nameof(Name), $"{nameof(Name)} cannot be empty!");
        }
    }

    private TimeSpan _videoLength;
    public string VideoLength
    {
        get => _videoLength.ToString();
        set
        {
            var parseResult = !TimeSpan.TryParse(value, out TimeSpan time);

            _videoLength = time;

            OnPropertyChanged();

            ClearErrors(nameof(VideoLength));

            if (time.TotalSeconds < 60) AddError(nameof(VideoLength), "Video cannot be zero minutes long!");
        }
    }

    private string _description;
    public string Description
    {
        get => _description;
        set
        {
            _description = value;

            OnPropertyChanged();

            ClearErrors(nameof(Description));

            if (string.IsNullOrWhiteSpace(_description)) AddError(nameof(Description), $"{nameof(Description)} cannot be empty!");
        }
    }

    private string _videoUrl;
    public string VideoUrl
    {
        get => _videoUrl;
        set
        {
            _videoUrl = value;

            OnPropertyChanged();

            ClearErrors(nameof(VideoUrl));

            if (string.IsNullOrWhiteSpace(_videoUrl)) { AddError(nameof(VideoUrl), $"{nameof(VideoUrl).Replace("Url", string.Empty)} path cannot be empty!"); return; }
            else if (_videoUrl[1] != ':') return;
            else if (!File.Exists(_videoUrl)) { AddError(nameof(VideoUrl), "File with this path not exists!"); return; }

            var size = Convert.ToDecimal(new FileInfo(_videoUrl).Length) / (1024 * 1024 * 1024);

            if (size > FileDialogService.MaxVideoSize) AddError(nameof(VideoUrl), $"File size cannot exceed {FileDialogService.MaxVideoSize}gb");
        }
    }

    private string _imageUrl;
    public string ImageUrl
    {
        get => _imageUrl;
        set
        {
            _imageUrl = value;

            OnPropertyChanged();

            ClearErrors(nameof(ImageUrl));

            if (string.IsNullOrWhiteSpace(_imageUrl)) { AddError(nameof(ImageUrl), $"{nameof(ImageUrl).Replace("Url", string.Empty)} path cannot be empty!"); return; }
            else if (_imageUrl[1] != ':') return;
            else if (!File.Exists(_imageUrl)) { AddError(nameof(ImageUrl), "File with this path not exists!"); return; }

            var size = Convert.ToDecimal(new FileInfo(_imageUrl).Length) / 1024;

            if (size > FileDialogService.MaxImageSize) AddError(nameof(ImageUrl), $"File size cannot exceed {FileDialogService.MaxImageSize / 1024}mb");
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

    private Season _season;
    public Season Season
    {
        get => _season;
        set
        {
            _season = value;

            OnPropertyChanged();

            ClearErrors(nameof(Season));

            if (_season is null) AddError(nameof(Season), $"{nameof(Season)} cannot be empty!");
        }
    }


    private int _videoProgress;
    public int VideoProgress
    {
        get => _videoProgress;
        set { _videoProgress = value; OnPropertyChanged(); }
    }

    private BlobStorageUploadProgress _imageProgress;
    public BlobStorageUploadProgress ImageProgress
    {
        get => _imageProgress;
        set { _imageProgress = value; OnPropertyChanged(); }
    }

    public bool VideoUploadSuccess { get; set; }
    public bool ImageUploadSuccess { get; set; }


    public override void Verify()
    {
        Name = Name is null ? Name : Name.Trim();
        Description = Description is null ? Description : Description.Trim();
        VideoLength = VideoLength;
        VideoUrl = VideoUrl;
        ImageUrl = ImageUrl;
    }

    public void VideoProgressEvent(object sender, UploadProgressArgs e)
    {
        var progress = (int)(e.TransferredBytes * 100 / e.TotalBytes);

        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            VideoProgress = progress;
        });
    }
}
