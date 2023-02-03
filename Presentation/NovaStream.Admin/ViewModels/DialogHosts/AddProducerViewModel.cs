namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddProducerViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;

    public Producer Producer { get; set; }

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
        DependencyProperty.Register("ImageProgress", typeof(BlobStorageUploadProgress), typeof(AddProducerViewModel));


    public AddProducerViewModel(AppDbContext dbContext, IStorageManager storageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;

        Producer = new Producer();

        UploadTasks = new();
        UploadTaskTokens = new();

        SaveCommand = new RelayCommand(() => Save());
        CancelCommand = new RelayCommand(() => Cancel());

        OpenImageDialogCommand = new RelayCommand(() => OpenImageDialog());
    }


    private async Task Save()
    {
        Producer? dbProducer = _dbContext.Producers.FirstOrDefault(a => a.Name == Producer.Name);

        UploadTasks.Clear();
        UploadTaskTokens.Clear();

        ImageProgressCompleted = false;

        // Actor ImageUrl
        if (dbProducer is null || dbProducer is not null && dbProducer.ImageUrl != Producer.ImageUrl)
        {
            var imageStream = new FileStream(Producer.ImageUrl, FileMode.Open, FileAccess.Read);
            Producer.ImageUrl = string.Format("Images/Producers/{0}-image{1}".Replace(' ', '-'), Producer.Name, Path.GetExtension(Producer.ImageUrl));

            ImageProgress = new BlobStorageUploadProgress(imageStream.Length);

            var imageToken = new CancellationTokenSource();
            var imageUploadTask = _storageManager.UploadFileAsync(imageStream, Producer.ImageUrl, ImageProgress, imageToken.Token);

            UploadTasks.Add(imageUploadTask);
            UploadTaskTokens.Add(imageToken);

            imageUploadTask.ContinueWith(_ => ImageProgressCompleted = true);
        }
        else ImageProgressCompleted = true;


        if (UploadTasks.Count > 0) await Task.WhenAll(UploadTasks);

        if (dbProducer is not null) _dbContext.Producers.Remove(dbProducer);

        _dbContext.Producers.Add(Producer);
        await _dbContext.SaveChangesAsync();
    }

    private async Task Cancel()
    {
        UploadTaskTokens.ForEach(ts => ts.Cancel());

        ImageProgress.Progress = 0;
        if (ImageProgressCompleted) await _storageManager.DeleteFileAsync(Producer.ImageUrl);
    }

    private void OpenImageDialog()
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "PNG Files(*.png)|*.png|JPEG Files(*.jpeg)|*.jpeg|JPG Files(*.jpg)|*.jpg";
        fileDialog.FilterIndex = 3;

        if (fileDialog.ShowDialog() is false) return;

        Producer.ImageUrl = fileDialog.FileName;
    }
}
