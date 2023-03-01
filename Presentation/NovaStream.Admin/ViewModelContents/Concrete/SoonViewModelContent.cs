namespace NovaStream.Admin.ViewModelContents.Concrete;

public class SoonViewModelContent : ViewModelContentBase
{
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

    private DateTime _outDate = DateTime.Now;
    public DateTime OutDate
    {
        get => _outDate;
        set
        {
            _outDate = value;

            OnPropertyChanged();

            ClearErrors(nameof(OutDate));

            if (_outDate < DateTime.Now) AddError(nameof(OutDate), "The release date of the movie or serial cannot be earlier than now!");
        }
    }

    private string _trailerUrl;
    public string TrailerUrl
    {
        get => _trailerUrl;
        set
        {
            _trailerUrl = value;

            OnPropertyChanged();

            ClearErrors(nameof(TrailerUrl));

            if (string.IsNullOrWhiteSpace(_trailerUrl)) { AddError(nameof(TrailerUrl), $"{nameof(TrailerUrl).Replace("Url", string.Empty)} path cannot be empty!"); return; }
            else if (_trailerUrl[1] != ':') return;
            else if (!File.Exists(_trailerUrl)) { AddError(nameof(TrailerUrl), "File with this path not exists!"); return; }

            var size = Convert.ToDecimal(new FileInfo(_trailerUrl).Length) / (1024 * 1024);

            if (size > FileDialogService.MaxTrailerSize) AddError(nameof(TrailerUrl), $"File size cannot exceed {FileDialogService.MaxTrailerSize}mb");
        }
    }

    private string _trailerImageUrl;
    public string TrailerImageUrl
    {
        get => _trailerImageUrl;
        set
        {
            _trailerImageUrl = value;

            OnPropertyChanged();

            ClearErrors(nameof(TrailerImageUrl));

            if (string.IsNullOrWhiteSpace(_trailerImageUrl)) { AddError(nameof(TrailerImageUrl), "Trailer Image path cannot be empty!"); return; }
            else if (_trailerImageUrl[1] != ':') return;
            else if (!File.Exists(_trailerImageUrl)) { AddError(nameof(TrailerImageUrl), "File with this path not exists!"); return; }

            var size = Convert.ToDecimal(new FileInfo(_trailerImageUrl).Length) / 1024;

            if (size > FileDialogService.MaxImageSize) AddError(nameof(TrailerImageUrl), $"File size cannot exceed {FileDialogService.MaxImageSize / 1024}mb");
        }
    }


    private BlobStorageUploadProgress _trailerProgress;
    public BlobStorageUploadProgress TrailerProgress
    {
        get => _trailerProgress;
        set { _trailerProgress = value; OnPropertyChanged(); }
    }

    private BlobStorageUploadProgress _trailerImageProgress;
    public BlobStorageUploadProgress TrailerImageProgress
    {
        get => _trailerImageProgress;
        set { _trailerImageProgress = value; OnPropertyChanged(); }
    }


    public bool TrailerUploadSuccess { get; set; }
    public bool TrailerImageUploadSuccess { get; set; }


    public override void Verify()
    {
        Name = Name is null ? Name : Name.Trim();
        Description = Description is null ? Description : Description.Trim();
        OutDate = OutDate;
        TrailerUrl = TrailerUrl;
        TrailerImageUrl = TrailerImageUrl;
    }
}
