namespace NovaStream.Admin.Models.Concrete;

public class UploadGenreModel : ModelBase
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

            if (string.IsNullOrWhiteSpace(_name)) AddError(nameof(Name), "Name cannot be empty!");
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

            if (string.IsNullOrWhiteSpace(_imageUrl)) { AddError(nameof(ImageUrl), "Image path cannot be empty!"); return; }

            else if (_imageUrl[1] != ':') return;
            else if (!File.Exists(_imageUrl)) { AddError(nameof(ImageUrl), "File with this path not exists!"); return; }

            var size = Convert.ToDecimal(new FileInfo(_imageUrl).Length) / 1024;

            if (size > 2048) AddError(nameof(ImageUrl), "File size cannot exceed 2mb");
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
        Name = Name;
        ImageUrl = ImageUrl;
    }
}
