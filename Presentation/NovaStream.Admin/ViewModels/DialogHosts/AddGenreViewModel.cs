namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddGenreViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;

    public Genre Genre { get; set; }

    public RelayCommand SaveCommand { get; set; }
    public RelayCommand CancelCommand { get; set; }
    public RelayCommand OpenImageDialogCommand { get; set; }


    public List<Task> UploadTasks { get; set; }
    public List<CancellationTokenSource> UploadTaskTokens { get; set; }


    public bool ImageProgressCompleted { get; set; }
    public BlobStorageUploadProgress ImageProgress
    {
        get { return (BlobStorageUploadProgress)GetValue(ImageProgressProperty); }
        set { SetValue(ImageProgressProperty, value); }
    }
    public static readonly DependencyProperty ImageProgressProperty =
        DependencyProperty.Register("ImageProgress", typeof(BlobStorageUploadProgress), typeof(AddGenreViewModel));


    public AddGenreViewModel(AppDbContext dbContext, IStorageManager storageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;

        _dbContext = dbContext;
        _storageManager = storageManager;

        Genre = new Genre();

        UploadTasks = new();
        UploadTaskTokens = new();

        SaveCommand = new RelayCommand(() => Save());
        CancelCommand = new RelayCommand(() => Cancel());

        OpenImageDialogCommand = new RelayCommand(() => OpenImageDialog());
    }


    private async Task Save()
    {
        Genre? dbGenre = _dbContext.Genres.FirstOrDefault(a => a.Name == Genre.Name);

        UploadTasks.Clear();
        UploadTaskTokens.Clear();

        ImageProgressCompleted = false;

        // Genre ImageUrl
        if (dbGenre is null || dbGenre is not null && dbGenre.ImageUrl != Genre.ImageUrl)
        {
            var imageStream = new FileStream(Genre.ImageUrl, FileMode.Open, FileAccess.Read);
            Genre.ImageUrl = string.Format("Images/Genres/{0}-image{1}".Replace(' ', '-'), Genre.Name, Path.GetExtension(Genre.ImageUrl));

            ImageProgress = new BlobStorageUploadProgress(imageStream.Length);

            var imageToken = new CancellationTokenSource();
            var imageUploadTask = _storageManager.UploadFileAsync(imageStream, Genre.ImageUrl, ImageProgress, imageToken.Token);

            UploadTasks.Add(imageUploadTask);
            UploadTaskTokens.Add(imageToken);

            imageUploadTask.ContinueWith(_ => ImageProgressCompleted = true);
        }
        else ImageProgressCompleted = true;


        if (UploadTasks.Count > 0) await Task.WhenAll(UploadTasks);

        if (dbGenre is not null) _dbContext.Genres.Remove(dbGenre);

        _dbContext.Genres.Add(Genre);
        await _dbContext.SaveChangesAsync();
    }

    private async Task Cancel()
    {
        UploadTaskTokens.ForEach(ts => ts.Cancel());

        ImageProgress.Progress = 0;
        if (ImageProgressCompleted) await _storageManager.DeleteFileAsync(Genre.ImageUrl);
    }

    private void OpenImageDialog()
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "PNG Files(*.png)|*.png|JPEG Files(*.jpeg)|*.jpeg|JPG Files(*.jpg)|*.jpg";
        fileDialog.FilterIndex = 3;

        if (fileDialog.ShowDialog() is false) return;

        Genre.ImageUrl = fileDialog.FileName;
    }
}
