namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddActorViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;

    public Actor Actor { get; set; }

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
        DependencyProperty.Register("ImageProgress", typeof(BlobStorageUploadProgress), typeof(AddActorViewModel));

    public AddActorViewModel(AppDbContext dbContext, IStorageManager storageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;

        Actor = new Actor();

        UploadTasks = new();
        UploadTaskTokens = new();

        SaveCommand = new RelayCommand(() => Save());
        CancelCommand = new RelayCommand(() => Cancel());

        OpenImageDialogCommand = new RelayCommand(() => OpenImageDialog());
    }

    private async Task Save()
    {
        Actor? dbActor = _dbContext.Actors.FirstOrDefault(a => a.Name == Actor.Name);

        UploadTasks.Clear();
        UploadTaskTokens.Clear();

        ImageProgressCompleted = false; 

        // Actor ImageUrl
        if (dbActor is null || dbActor is not null && dbActor.ImageUrl != Actor.ImageUrl)
        {
            var imageStream = new FileStream(Actor.ImageUrl, FileMode.Open, FileAccess.Read);
            Actor.ImageUrl = string.Format("Images/Actors/{0}-image{1}".Replace(' ', '-'), Actor.Name, Path.GetExtension(Actor.ImageUrl));

            ImageProgress = new BlobStorageUploadProgress(imageStream.Length);

            var imageToken = new CancellationTokenSource();
            var imageUploadTask = _storageManager.UploadFileAsync(imageStream, Actor.ImageUrl, ImageProgress, imageToken.Token);

            UploadTasks.Add(imageUploadTask);
            UploadTaskTokens.Add(imageToken);

            imageUploadTask.ContinueWith(_ => ImageProgressCompleted = true);
        }
        else ImageProgressCompleted = true;


        if (UploadTasks.Count > 0) await Task.WhenAll(UploadTasks);

        if (dbActor is not null) _dbContext.Actors.Remove(dbActor);

        _dbContext.Actors.Add(Actor);
        await _dbContext.SaveChangesAsync();
    }

    private async Task Cancel()
    {
        UploadTaskTokens.ForEach(ts => ts.Cancel());

        ImageProgress.Progress = 0;
        if (ImageProgressCompleted) await _storageManager.DeleteFileAsync(Actor.ImageUrl);
    }

    private void OpenImageDialog()
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "PNG Files(*.png)|*.png|JPEG Files(*.jpeg)|*.jpeg|JPG Files(*.jpg)|*.jpg";
        fileDialog.FilterIndex = 3;

        if (fileDialog.ShowDialog() is false) return;

        Actor.ImageUrl = fileDialog.FileName;
    }
}
