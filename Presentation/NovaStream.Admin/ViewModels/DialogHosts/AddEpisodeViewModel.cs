using NovaStream.Domain.Entities.Concrete;

namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddEpisodeViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;
    private readonly IAWSStorageManager _awsStorageManager;

    public Episode Episode { get; set; }

    public RelayCommand SaveCommand { get; set; }
    public RelayCommand CancelCommand { get; set; }
    public RelayCommand OpenVideoDialogCommand { get; set; }
    public RelayCommand OpenVideoImageDialogCommand { get; set; }


    public List<Task> UploadTasks { get; set; }
    public List<CancellationTokenSource> UploadTaskTokens { get; set; }


    public bool VideoProgressCompleted { get; set; }
    public int VideoProgress
    {
        get { return (int)GetValue(VideoProgressProperty); }
        set { SetValue(VideoProgressProperty, value); }
    }
    public static readonly DependencyProperty VideoProgressProperty =
        DependencyProperty.Register("VideoProgress", typeof(int), typeof(AddEpisodeViewModel));

    public bool VideoImageProgressCompleted { get; set; }
    public BlobStorageUploadProgress VideoImageProgress
    {
        get { return (BlobStorageUploadProgress)GetValue(VideoImageProgressProperty); }
        set { SetValue(VideoImageProgressProperty, value); }
    }
    public static readonly DependencyProperty VideoImageProgressProperty =
        DependencyProperty.Register("VideoImageProgress", typeof(BlobStorageUploadProgress), typeof(AddEpisodeViewModel));


    public AddEpisodeViewModel(AppDbContext dbContext, IStorageManager storageManager, IAWSStorageManager awsStorageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;
        _awsStorageManager = awsStorageManager;

        Episode = new Episode();

        UploadTasks = new();
        UploadTaskTokens = new();

        SaveCommand = new RelayCommand(() => Save());
        CancelCommand = new RelayCommand(() => Cancel());

        OpenVideoDialogCommand = new RelayCommand(() => OpenVideoDialog());
        OpenVideoImageDialogCommand = new RelayCommand(() => OpenVideoImageDialog());

    }


    private async Task Save()
    {
        await Task.CompletedTask;

        UploadTasks.Clear();
        UploadTaskTokens.Clear();

        VideoProgressCompleted = false;
        VideoImageProgressCompleted = false;

        // Episode VideoUrl
        var videoStream = new FileStream(Episode.VideoUrl, FileMode.Open, FileAccess.Read);
        //Episode.VideoUrl = string.Format("Serials/{0}/Season {1}/Episode {2}/{3}-video{4}", Episode.Serial, Episode.Season.Id, Episode.Number, Path.GetFileNameWithoutExtension(Episode.VideoUrl), Path.GetExtension(Episode.VideoUrl)); 
        // !!!!!!!!!!!!!!!!!!!!!!!! PROBLEM !!!!!!!!!!!!!!!!!!!!!!!!

        var videoToken = new CancellationTokenSource();
        var videoUploadTask = _awsStorageManager.UploadFileAsync(videoStream, Episode.VideoUrl, VideoProgressEvent, videoToken.Token);

        UploadTasks.Add(videoUploadTask);
        UploadTaskTokens.Add(videoToken);

        videoUploadTask.ContinueWith(_ => VideoProgressCompleted = true);
    }

    private async Task Cancel()
    {
        UploadTaskTokens.ForEach(ts => ts.Cancel());

        System.Windows.Application.Current.Dispatcher.Invoke(() => VideoProgress = 0);
        if (VideoProgressCompleted) await _awsStorageManager.DeleteFileAsync(Episode.VideoUrl);

        VideoImageProgress.Progress = 0;
        if (VideoImageProgressCompleted) await _storageManager.DeleteFileAsync(Episode.ImageUrl);
    }

    private void OpenVideoDialog()
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "MP4 Files(*.mp4)|*.mp4|AVI Files(*.avi)|*.avi|MOV Files(*.mov)|*.mov";
        fileDialog.FilterIndex = 1;

        if (fileDialog.ShowDialog() is false) return;

        Episode.VideoUrl = fileDialog.FileName;
    }

    private void OpenVideoImageDialog()
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "PNG Files(*.png)|*.png|JPEG Files(*.jpeg)|*.jpeg|JPG Files(*.jpg)|*.jpg";
        fileDialog.FilterIndex = 3;

        if (fileDialog.ShowDialog() is false) return;

        Episode.ImageUrl = fileDialog.FileName;
    }

    private void VideoProgressEvent(object sender, UploadProgressArgs e)
    {
        var progress = (int)(e.TransferredBytes * 100 / e.TotalBytes);

        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            VideoProgress = progress;
        });
    }
}
