namespace NovaStream.Admin.ViewModelContents.Concrete;

public class ActorViewModelContent : ViewModelContentBase
{
    public int Id { get; set; }

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

    private string _surname;
    public string Surname
    {
        get => _surname;
        set
        {
            _surname = value;

            OnPropertyChanged();

            ClearErrors(nameof(Surname));

            if (string.IsNullOrWhiteSpace(_surname)) AddError(nameof(Surname), $"{nameof(Surname)} cannot be empty!");
        }
    }

    private string _about;
    public string About
    {
        get => _about;
        set
        {
            _about = value;

            OnPropertyChanged();

            ClearErrors(nameof(About));

            if (string.IsNullOrWhiteSpace(_about)) AddError(nameof(About), $"{nameof(About)} cannot be empty!");
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


    private BlobStorageUploadProgress _imageProgress;
    public BlobStorageUploadProgress ImageProgress
    {
        get => _imageProgress;
        set { _imageProgress = value; OnPropertyChanged(); }
    }


    public bool ImageUploadSuccess { get; set; }


    public override void Verify()
    {
        Name = Name is null ? Name : Name.Trim();
        Surname = Surname is null ? Surname : Surname.Trim();
        About = About is null ? About : About.Trim();
        ImageUrl = ImageUrl;
    }
}
