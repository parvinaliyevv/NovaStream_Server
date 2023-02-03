namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddEpisodeViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;
    private readonly IAWSStorageManager _awsStorageManager;

    public Serial Serial { get; set; }
    public Episode Episode { get; set; }

    public List<Serial> Serials { get; set; }


    public int Number
    {
        get { return (int)GetValue(NumberProperty); }
        set { SetValue(NumberProperty, value); }
    }
    public static readonly DependencyProperty NumberProperty =
        DependencyProperty.Register("Number", typeof(int), typeof(AddEpisodeViewModel));

    public Season Season
    {
        get { return (Season)GetValue(SeasonProperty); }
        set { SetValue(SeasonProperty, value); }
    }
    public static readonly DependencyProperty SeasonProperty =
        DependencyProperty.Register("Season", typeof(Season), typeof(AddEpisodeViewModel));

    public List<Season> Seasons
    {
        get { return (List<Season>)GetValue(SeasonsProperty); }
        set { SetValue(SeasonsProperty, value); }
    }
    public static readonly DependencyProperty SeasonsProperty =
        DependencyProperty.Register("Seasons", typeof(List<Season>), typeof(AddEpisodeViewModel));

    public RelayCommand SaveCommand { get; set; }
    public RelayCommand CancelCommand { get; set; }
    public RelayCommand OpenVideoDialogCommand { get; set; }
    public RelayCommand OpenVideoImageDialogCommand { get; set; }
    public RelayCommand SelectedSerialChangedCommand { get; set; }
    public RelayCommand SelectedSeasonChangedCommand { get; set; }


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

        Serials = _dbContext.Serials.ToList();
        Serial = Serials.FirstOrDefault();

        Seasons = _dbContext.Seasons.Where(s => s.SerialName == Serial.Name).ToList();
        Season = Seasons.FirstOrDefault();

        UploadTasks = new();
        UploadTaskTokens = new();

        SaveCommand = new RelayCommand(() => Save());
        CancelCommand = new RelayCommand(() => Cancel());

        OpenVideoDialogCommand = new RelayCommand(() => OpenVideoDialog());
        OpenVideoImageDialogCommand = new RelayCommand(() => OpenVideoImageDialog());

        SelectedSerialChangedCommand = new RelayCommand(() => SelectedSerialChanged());
        SelectedSeasonChangedCommand = new RelayCommand(() => SelectedSeasonChanged());
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

        Episode.Number = Number; // number auto dusur tut bil Parvin
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

    private void SelectedSerialChanged()
    {
        Seasons = _dbContext.Seasons.Where(s => s.SerialName == Serial.Name).ToList();
        Season = Seasons.FirstOrDefault();
    }

    private void SelectedSeasonChanged() // Bug method!
    {
        //if (Season is null) Season = Seasons.FirstOrDefault(); 

        //var lastEpisodeNumber = _dbContext.Episodes.Where(e => e.SeasonId == Season.Id)
        //    .OrderBy(e => e.Number).Last().Number;

        //Number = lastEpisodeNumber + 1;
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
