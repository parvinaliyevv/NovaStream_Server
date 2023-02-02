namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddSerialViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;
    private readonly IAWSStorageManager _awsStorageManager;


    public Serial Serial { get; set; }
    public Season Season { get; set; }
    public Episode Episode { get; set; }
    public List<Producer> Producers { get; set; }


    public RelayCommand SaveCommand { get; set; }
    public RelayCommand CancelCommand { get; set; }
    public RelayCommand OpenVideoDialogCommand { get; set; }
    public RelayCommand OpenVideoImageDialogCommand { get; set; }
    public RelayCommand OpenTrailerDialogCommand { get; set; }
    public RelayCommand OpenImageDialogCommand { get; set; }
    public RelayCommand OpenSearchImageDialogCommand { get; set; }


    public List<Task> UploadTasks { get; set; }
    public List<CancellationTokenSource> UploadTaskTokens { get; set; }


    public bool VideoProgressCompleted { get; set; }
    public int VideoProgress
    {
        get { return (int)GetValue(VideoProgressProperty); }
        set { SetValue(VideoProgressProperty, value); }
    }
    public static readonly DependencyProperty VideoProgressProperty =
        DependencyProperty.Register("VideoProgress", typeof(int), typeof(AddSerialViewModel));

    public bool TrailerProgressCompleted { get; set; }
    public BlobStorageUploadProgress TrailerProgress
    {
        get { return (BlobStorageUploadProgress)GetValue(TrailerProgressProperty); }
        set { SetValue(TrailerProgressProperty, value); }
    }
    public static readonly DependencyProperty TrailerProgressProperty =
        DependencyProperty.Register("TrailerProgress", typeof(BlobStorageUploadProgress), typeof(AddSerialViewModel));

    public bool VideoImageProgressCompleted { get; set; }
    public BlobStorageUploadProgress VideoImageProgress
    {
        get { return (BlobStorageUploadProgress)GetValue(VideoImageProgressProperty); }
        set { SetValue(VideoImageProgressProperty, value); }
    }
    public static readonly DependencyProperty VideoImageProgressProperty =
        DependencyProperty.Register("VideoImageProgress", typeof(BlobStorageUploadProgress), typeof(AddSerialViewModel));

    public bool ImageProgressCompleted { get; set; }
    public BlobStorageUploadProgress ImageProgress
    {
        get { return (BlobStorageUploadProgress)GetValue(ImageProgressProperty); }
        set { SetValue(ImageProgressProperty, value); }
    }
    public static readonly DependencyProperty ImageProgressProperty =
        DependencyProperty.Register("ImageProgress", typeof(BlobStorageUploadProgress), typeof(AddSerialViewModel));

    public bool SearchImageProgressCompleted { get; set; }
    public BlobStorageUploadProgress SearchImageProgress
    {
        get { return (BlobStorageUploadProgress)GetValue(SearchImageProgressProperty); }
        set { SetValue(SearchImageProgressProperty, value); }
    }
    public static readonly DependencyProperty SearchImageProgressProperty =
        DependencyProperty.Register("SearchImageProgress", typeof(BlobStorageUploadProgress), typeof(AddSerialViewModel));


    public AddSerialViewModel(AppDbContext dbContext, IStorageManager storageManager, IAWSStorageManager awsStorageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;
        _awsStorageManager = awsStorageManager;

        Serial = new Serial();
        Season = new Season();
        Episode = new Episode();
        Producers = dbContext.Producers.ToList();

        UploadTasks = new();
        UploadTaskTokens = new();

        SaveCommand = new RelayCommand(() => Save());
        CancelCommand = new RelayCommand(() => Cancel());
        
        OpenVideoDialogCommand = new RelayCommand(() => OpenVideoDialog());
        OpenVideoImageDialogCommand = new RelayCommand(() => OpenVideoImageDialog());
        OpenTrailerDialogCommand = new RelayCommand(() => OpenTrailerDialog());
        OpenImageDialogCommand = new RelayCommand(() => OpenImageDialog());
        OpenSearchImageDialogCommand = new RelayCommand(() => OpenSearchImageDialog());
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        Serial? dbSerial = _dbContext.Serials.FirstOrDefault(s => s.Name == Serial.Name);
        Season? dbSeason = null;
        Episode? dbEpisode = null;
        
        if (dbSerial is not null)
        {
            dbSeason = _dbContext.Seasons.FirstOrDefault(s => s.SerialName == Serial.Name);

            if (dbSerial is not null) 
                dbEpisode = _dbContext.Episodes.FirstOrDefault(e => e.Id == dbSeason.Id);
        }

        UploadTasks.Clear();
        UploadTaskTokens.Clear();

        VideoProgressCompleted = false;
        VideoImageProgressCompleted = false;
        TrailerProgressCompleted = false;
        ImageProgressCompleted = false;
        SearchImageProgressCompleted = false;


        // Episode VideoUrl
        if (dbEpisode is null || dbEpisode is not null && dbEpisode.VideoUrl != Episode.VideoUrl)
        {
            using var videoStream = new FileStream(Episode.VideoUrl, FileMode.Open, FileAccess.Read);
            Episode.VideoUrl = string.Format("Serials/{0}/Season 1/Episode 1/{1}-video{2}", Serial.Name, Path.GetFileNameWithoutExtension(Episode.VideoUrl), Path.GetExtension(Episode.VideoUrl));

            var videoToken = new CancellationTokenSource();
            var videoUploadTask = _awsStorageManager.UploadFileAsync(videoStream, Episode.VideoUrl, VideoProgressEvent, videoToken.Token);

            UploadTasks.Add(videoUploadTask);
            UploadTaskTokens.Add(videoToken);

            videoUploadTask.ContinueWith(_ => VideoProgressCompleted = true);
        }
        else VideoProgressCompleted = true;

        // Serial TrailerUrl
        if (dbEpisode is null || dbEpisode is not null && dbEpisode.ImageUrl != Episode.ImageUrl)
        {
            using var videoImageStream = new FileStream(Episode.ImageUrl, FileMode.Open, FileAccess.Read);
            Episode.ImageUrl = string.Format("Serials/{0}/Season 1/Episode 1/{1}-video-image{2}", Serial.Name, Path.GetFileNameWithoutExtension(Episode.ImageUrl), Path.GetExtension(Episode.ImageUrl));

            VideoImageProgress = new BlobStorageUploadProgress(videoImageStream.Length);

            var videoImageToken = new CancellationTokenSource();
            var videoImageUploadTask = _storageManager.UploadFileAsync(videoImageStream, Episode.ImageUrl, VideoImageProgress, videoImageToken.Token);

            UploadTasks.Add(videoImageUploadTask);
            UploadTaskTokens.Add(videoImageToken);

            videoImageUploadTask.ContinueWith(_ => VideoImageProgressCompleted = true);
        }
        else VideoImageProgressCompleted = true;

        // Episode ImageUrl
        if (dbSerial is null || dbSerial is not null && dbSerial.TrailerUrl != Serial.TrailerUrl)
        {
            using var trailerStream = new FileStream(Serial.TrailerUrl, FileMode.Open, FileAccess.Read);
            Serial.TrailerUrl = string.Format("Serials/{0}/{1}-trailer{2}", Serial.Name, Path.GetFileNameWithoutExtension(Serial.TrailerUrl), Path.GetExtension(Serial.TrailerUrl));

            TrailerProgress = new BlobStorageUploadProgress(trailerStream.Length);

            var trailerToken = new CancellationTokenSource();
            var trailerUploadTask = _storageManager.UploadFileAsync(trailerStream, Serial.TrailerUrl, TrailerProgress, trailerToken.Token);

            UploadTasks.Add(trailerUploadTask);
            UploadTaskTokens.Add(trailerToken);

            trailerUploadTask.ContinueWith(_ => TrailerProgressCompleted = true);
        }
        else TrailerProgressCompleted = true;

        // Serial ImageUrl
        if (dbSerial is null || dbSerial is not null && dbSerial.ImageUrl != Serial.ImageUrl)
        {
            using var imageStream = new FileStream(Serial.ImageUrl, FileMode.Open, FileAccess.Read);
            Serial.ImageUrl = string.Format("Serials/{0}/{1}-image{2}", Serial.Name, Path.GetFileNameWithoutExtension(Serial.ImageUrl), Path.GetExtension(Serial.ImageUrl));

            ImageProgress = new BlobStorageUploadProgress(imageStream.Length);

            var imageToken = new CancellationTokenSource();
            var imageUploadTask = _storageManager.UploadFileAsync(imageStream, Serial.ImageUrl, ImageProgress, imageToken.Token);

            UploadTasks.Add(imageUploadTask);
            UploadTaskTokens.Add(imageToken);

            imageUploadTask.ContinueWith(_ => ImageProgressCompleted = true);
        }
        else ImageProgressCompleted = true;

        // Serial SearchImageUrl
        if (dbSerial is null || dbSerial is not null && dbSerial.SearchImageUrl != Serial.SearchImageUrl)
        {
            using var searchImageStream = new FileStream(Serial.SearchImageUrl, FileMode.Open, FileAccess.Read);
            Serial.SearchImageUrl = string.Format("Serials/{0}/{1}-search-image{2}", Serial.Name, Path.GetFileNameWithoutExtension(Serial.SearchImageUrl), Path.GetExtension(Serial.SearchImageUrl));

            SearchImageProgress = new BlobStorageUploadProgress(searchImageStream.Length);

            var searchImageToken = new CancellationTokenSource();
            var searchImageUploadTask = _storageManager.UploadFileAsync(searchImageStream, Serial.SearchImageUrl, SearchImageProgress, searchImageToken.Token);

            UploadTasks.Add(searchImageUploadTask);
            UploadTaskTokens.Add(searchImageToken);

            searchImageUploadTask.ContinueWith(_ => SearchImageProgressCompleted = true);
        }
        else SearchImageProgressCompleted = true;


        if (UploadTasks.Count > 0) await Task.WhenAll(UploadTasks);

        if (dbSerial is not null) _dbContext.Serials.Remove(dbSerial);

        _dbContext.Serials.Add(Serial);
        await _dbContext.SaveChangesAsync();

        Season.Number = 1;
        Season.SerialName = Serial.Name;

        _dbContext.Seasons.Add(Season);
        await _dbContext.SaveChangesAsync();

        Episode.Number = 1;
        Episode.SeasonId = Season.Id;

        _dbContext.Episodes.Add(Episode);
        await _dbContext.SaveChangesAsync();
    }

    private async Task Cancel()
    {
        UploadTaskTokens.ForEach(ts => ts.Cancel());

        System.Windows.Application.Current.Dispatcher.Invoke(() => VideoProgress = 0);
        if (VideoProgressCompleted) await _awsStorageManager.DeleteFileAsync(Episode.VideoUrl);

        VideoImageProgress.Progress = 0;
        if (VideoImageProgressCompleted) await _storageManager.DeleteFileAsync(Episode.ImageUrl);

        TrailerProgress.Progress = 0;
        if (VideoProgressCompleted) await _storageManager.DeleteFileAsync(Serial.TrailerUrl);

        ImageProgress.Progress = 0;
        if (VideoProgressCompleted) await _storageManager.DeleteFileAsync(Serial.ImageUrl);

        SearchImageProgress.Progress = 0;
        if (VideoProgressCompleted) await _storageManager.DeleteFileAsync(Serial.SearchImageUrl);
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

    private void OpenTrailerDialog()
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "MP4 Files(*.mp4)|*.mp4|AVI Files(*.avi)|*.avi|MOV Files(*.mov)|*.mov";
        fileDialog.FilterIndex = 1;

        if (fileDialog.ShowDialog() is false) return;

        Serial.TrailerUrl = fileDialog.FileName;
    }

    private void OpenImageDialog()
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "PNG Files(*.png)|*.png|JPEG Files(*.jpeg)|*.jpeg|JPG Files(*.jpg)|*.jpg";
        fileDialog.FilterIndex = 3;

        if (fileDialog.ShowDialog() is false) return;

        Serial.ImageUrl = fileDialog.FileName;
    }

    private void OpenSearchImageDialog()
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "PNG Files(*.png)|*.png|JPEG Files(*.jpeg)|*.jpeg|JPG Files(*.jpg)|*.jpg";
        fileDialog.FilterIndex = 3;

        if (fileDialog.ShowDialog() is false) return;

        Serial.SearchImageUrl = fileDialog.FileName;
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
