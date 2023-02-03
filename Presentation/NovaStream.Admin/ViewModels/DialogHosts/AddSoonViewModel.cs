namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddSoonViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;


    public Soon Soon { get; set; }


    public RelayCommand SaveCommand { get; set; }
    public RelayCommand CancelCommand { get; set; }
    public RelayCommand OpenTrailerDialogCommand { get; set; }
    public RelayCommand OpenTrailerImageDialogCommand { get; set; }


    public List<Task> UploadTasks { get; set; }
    public List<CancellationTokenSource> UploadTaskTokens { get; set; }


    public bool ProgressCompleted { get; set; }


    public bool TrailerProgressCompleted { get; set; }
    public BlobStorageUploadProgress TrailerProgress
    {
        get { return (BlobStorageUploadProgress)GetValue(TrailerProgressProperty); }
        set { SetValue(TrailerProgressProperty, value); }
    }
    public static readonly DependencyProperty TrailerProgressProperty =
        DependencyProperty.Register("TrailerProgress", typeof(BlobStorageUploadProgress), typeof(AddSoonViewModel));

    public bool TrailerImageProgressCompleted { get; set; }
    public BlobStorageUploadProgress TrailerImageProgress
    {
        get { return (BlobStorageUploadProgress)GetValue(TrailerImageProgressProperty); }
        set { SetValue(TrailerImageProgressProperty, value); }
    }
    public static readonly DependencyProperty TrailerImageProgressProperty =
        DependencyProperty.Register("TrailerImageProgress", typeof(BlobStorageUploadProgress), typeof(AddSoonViewModel));


    public AddSoonViewModel(AppDbContext dbContext, IStorageManager storageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;

        Soon = new Soon();

        UploadTasks = new();
        UploadTaskTokens = new();

        SaveCommand = new RelayCommand(() => Save());
        CancelCommand = new RelayCommand(() => Cancel());

        OpenTrailerDialogCommand = new RelayCommand(() => OpenTrailerDialog());
        OpenTrailerImageDialogCommand = new RelayCommand(() => OpenTrailerImageDialog());
    }


    private async Task Save()
    {
        Soon? dbSoon = _dbContext.Soons.FirstOrDefault(s => s.Name == Soon.Name);

        UploadTasks.Clear();
        UploadTaskTokens.Clear();

        TrailerProgressCompleted = false;
        TrailerImageProgressCompleted = false;

        // Soon TrailerUrl
        if (dbSoon is null || dbSoon is not null && dbSoon.TrailerUrl != Soon.TrailerUrl)
        {
            var trailerStream = new FileStream(Soon.TrailerUrl, FileMode.Open, FileAccess.Read);
            Soon.TrailerUrl = string.Format("Soons/{0}/{1}-trailer{2}", Soon.Name, Path.GetFileNameWithoutExtension(Soon.TrailerUrl), Path.GetExtension(Soon.TrailerUrl));

            TrailerProgress = new BlobStorageUploadProgress(trailerStream.Length);

            var trailerToken = new CancellationTokenSource();
            var trailerUploadTask = _storageManager.UploadFileAsync(trailerStream, Soon.TrailerUrl, TrailerProgress, trailerToken.Token);

            UploadTasks.Add(trailerUploadTask);
            UploadTaskTokens.Add(trailerToken);

            trailerUploadTask.ContinueWith(_ => TrailerProgressCompleted = true);
        }
        else TrailerProgressCompleted = true;

        // Soon TrailerImageUrl
        if (dbSoon is null || dbSoon is not null && dbSoon.TrailerImageUrl != Soon.TrailerImageUrl)
        {
            var imageStream = new FileStream(Soon.TrailerImageUrl, FileMode.Open, FileAccess.Read);
            Soon.TrailerImageUrl = string.Format("Soons/{0}/{1}-image{2}", Soon.Name, Path.GetFileNameWithoutExtension(Soon.TrailerImageUrl), Path.GetExtension(Soon.TrailerImageUrl));

            TrailerImageProgress = new BlobStorageUploadProgress(imageStream.Length);

            var imageToken = new CancellationTokenSource();
            var imageUploadTask = _storageManager.UploadFileAsync(imageStream, Soon.TrailerImageUrl, TrailerImageProgress, imageToken.Token);

            UploadTasks.Add(imageUploadTask);
            UploadTaskTokens.Add(imageToken);

            imageUploadTask.ContinueWith(_ => TrailerImageProgressCompleted = true);
        }
        else TrailerImageProgressCompleted = true;


        if (UploadTasks.Count > 0) await Task.WhenAll(UploadTasks);

        if (dbSoon is not null) _dbContext.Soons.Remove(dbSoon);

        _dbContext.Soons.Add(Soon);
        await _dbContext.SaveChangesAsync();
    }

    private async Task Cancel()
    {
        UploadTaskTokens.ForEach(ts => ts.Cancel());

        TrailerProgress.Progress = 0;
        if (ProgressCompleted) await _storageManager.DeleteFileAsync(Soon.TrailerUrl);

        TrailerImageProgress.Progress = 0;
        if (ProgressCompleted) await _storageManager.DeleteFileAsync(Soon.TrailerImageUrl);
    }

    private void OpenTrailerDialog()
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "MP4 Files(*.mp4)|*.mp4|AVI Files(*.avi)|*.avi|MOV Files(*.mov)|*.mov";
        fileDialog.FilterIndex = 1;

        if (fileDialog.ShowDialog() is false) return;

        Soon.TrailerUrl = fileDialog.FileName;
    }

    private void OpenTrailerImageDialog()
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "PNG Files(*.png)|*.png|JPEG Files(*.jpeg)|*.jpeg|JPG Files(*.jpg)|*.jpg";
        fileDialog.FilterIndex = 3;

        if (fileDialog.ShowDialog() is false) return;

        Soon.TrailerImageUrl = fileDialog.FileName;
    }
}
