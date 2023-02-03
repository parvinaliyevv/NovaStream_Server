namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddMovieViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;
    private readonly IAWSStorageManager _awsStorageManager;


    public Movie Movie { get; set; }
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
        DependencyProperty.Register("VideoProgress", typeof(int), typeof(AddMovieViewModel));

    public bool TrailerProgressCompleted { get; set; }
    public BlobStorageUploadProgress TrailerProgress
    {
        get { return (BlobStorageUploadProgress)GetValue(TrailerProgressProperty); }
        set { SetValue(TrailerProgressProperty, value); }
    }
    public static readonly DependencyProperty TrailerProgressProperty =
        DependencyProperty.Register("TrailerProgress", typeof(BlobStorageUploadProgress), typeof(AddMovieViewModel));

    public bool VideoImageProgressCompleted { get; set; }
    public BlobStorageUploadProgress VideoImageProgress
    {
        get { return (BlobStorageUploadProgress)GetValue(VideoImageProgressProperty); }
        set { SetValue(VideoImageProgressProperty, value); }
    }
    public static readonly DependencyProperty VideoImageProgressProperty =
        DependencyProperty.Register("VideoImageProgress", typeof(BlobStorageUploadProgress), typeof(AddMovieViewModel));

    public bool ImageProgressCompleted { get; set; }
    public BlobStorageUploadProgress ImageProgress
    {
        get { return (BlobStorageUploadProgress)GetValue(ImageProgressProperty); }
        set { SetValue(ImageProgressProperty, value); }
    }
    public static readonly DependencyProperty ImageProgressProperty =
        DependencyProperty.Register("ImageProgress", typeof(BlobStorageUploadProgress), typeof(AddMovieViewModel));

    public bool SearchImageProgressCompleted { get; set; }
    public BlobStorageUploadProgress SearchImageProgress
    {
        get { return (BlobStorageUploadProgress)GetValue(SearchImageProgressProperty); }
        set { SetValue(SearchImageProgressProperty, value); }
    }
    public static readonly DependencyProperty SearchImageProgressProperty =
        DependencyProperty.Register("SearchImageProgress", typeof(BlobStorageUploadProgress), typeof(AddMovieViewModel));

    public AddMovieViewModel(AppDbContext dbContext, IStorageManager storageManager, IAWSStorageManager awsStorageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;
        _awsStorageManager = awsStorageManager;

        Movie = new Movie();
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
        Movie? dbMovie = _dbContext.Movies.FirstOrDefault(m => m.Name == Movie.Name);

        UploadTasks.Clear();
        UploadTaskTokens.Clear();

        VideoProgressCompleted = false;
        VideoImageProgressCompleted = false;
        TrailerProgressCompleted = false;
        ImageProgressCompleted = false;
        SearchImageProgressCompleted = false;

        // Movie VideoUrl
        if (dbMovie is null || dbMovie is not null && dbMovie.VideoUrl != Movie.VideoUrl)
        {
            var videoStream = new FileStream(Movie.VideoUrl, FileMode.Open, FileAccess.Read);
            Movie.VideoUrl = string.Format("Movies/{0}/{1}-video{2}", Movie.Name, Path.GetFileNameWithoutExtension(Movie.VideoUrl), Path.GetExtension(Movie.VideoUrl));

            var videoToken = new CancellationTokenSource();
            var videoUploadTask = _awsStorageManager.UploadFileAsync(videoStream, Movie.VideoUrl, VideoProgressEvent, videoToken.Token);

            UploadTasks.Add(videoUploadTask);
            UploadTaskTokens.Add(videoToken);

            videoUploadTask.ContinueWith(_ => VideoProgressCompleted = true);
        }
        else VideoProgressCompleted = true;

        // Movie VideoImageUrl
        if (dbMovie is null || dbMovie is not null && dbMovie.ImageUrl != Movie.ImageUrl)
        {
            var videoImageStream = new FileStream(Movie.VideoImageUrl, FileMode.Open, FileAccess.Read);
            Movie.VideoImageUrl = string.Format("Movies/{0}/{1}-video-image{2}", Movie.Name, Path.GetFileNameWithoutExtension(Movie.VideoImageUrl), Path.GetExtension(Movie.VideoImageUrl));

            VideoImageProgress = new BlobStorageUploadProgress(videoImageStream.Length);

            var videoImageToken = new CancellationTokenSource();
            var videoImageUploadTask = _storageManager.UploadFileAsync(videoImageStream, Movie.ImageUrl, VideoImageProgress, videoImageToken.Token);

            UploadTasks.Add(videoImageUploadTask);
            UploadTaskTokens.Add(videoImageToken);

            videoImageUploadTask.ContinueWith(_ => VideoImageProgressCompleted = true);
        }
        else VideoImageProgressCompleted = true;

        // Movie TrailerUrl
        if (dbMovie is null || dbMovie is not null && dbMovie.TrailerUrl != Movie.TrailerUrl)
        {
            var trailerStream = new FileStream(Movie.TrailerUrl, FileMode.Open, FileAccess.Read);
            Movie.TrailerUrl = string.Format("Movies/{0}/{1}-trailer{2}", Movie.Name, Path.GetFileNameWithoutExtension(Movie.TrailerUrl), Path.GetExtension(Movie.TrailerUrl));

            TrailerProgress = new BlobStorageUploadProgress(trailerStream.Length);

            var trailerToken = new CancellationTokenSource();
            var trailerUploadTask = _storageManager.UploadFileAsync(trailerStream, Movie.TrailerUrl, TrailerProgress, trailerToken.Token);

            UploadTasks.Add(trailerUploadTask);
            UploadTaskTokens.Add(trailerToken);

            trailerUploadTask.ContinueWith(_ => TrailerProgressCompleted = true);
        }
        else TrailerProgressCompleted = true;

        // Movie ImageUrl
        if (dbMovie is null || dbMovie is not null && dbMovie.ImageUrl != Movie.ImageUrl)
        {
            var imageStream = new FileStream(Movie.ImageUrl, FileMode.Open, FileAccess.Read);
            Movie.ImageUrl = string.Format("Movies/{0}/{1}-image{2}", Movie.Name, Path.GetFileNameWithoutExtension(Movie.ImageUrl), Path.GetExtension(Movie.ImageUrl));

            ImageProgress = new BlobStorageUploadProgress(imageStream.Length);

            var imageToken = new CancellationTokenSource();
            var imageUploadTask = _storageManager.UploadFileAsync(imageStream, Movie.ImageUrl, ImageProgress, imageToken.Token);

            UploadTasks.Add(imageUploadTask);
            UploadTaskTokens.Add(imageToken);

            imageUploadTask.ContinueWith(_ => ImageProgressCompleted = true);
        }
        else ImageProgressCompleted = true;

        // Serial SearchImageUrl
        if (dbMovie is null || dbMovie is not null && dbMovie.SearchImageUrl != Movie.SearchImageUrl)
        {
            var searchImageStream = new FileStream(Movie.SearchImageUrl, FileMode.Open, FileAccess.Read);
            Movie.SearchImageUrl = string.Format("Movies/{0}/{1}-search-image{2}", Movie.Name, Path.GetFileNameWithoutExtension(Movie.SearchImageUrl), Path.GetExtension(Movie.SearchImageUrl));

            SearchImageProgress = new BlobStorageUploadProgress(searchImageStream.Length);

            var searchImageToken = new CancellationTokenSource();
            var searchImageUploadTask = _storageManager.UploadFileAsync(searchImageStream, Movie.SearchImageUrl, SearchImageProgress, searchImageToken.Token);

            UploadTasks.Add(searchImageUploadTask);
            UploadTaskTokens.Add(searchImageToken);

            searchImageUploadTask.ContinueWith(_ => SearchImageProgressCompleted = true);
        }
        else SearchImageProgressCompleted = true;


        if (UploadTasks.Count > 0) await Task.WhenAll(UploadTasks);

        if (dbMovie is not null) _dbContext.Movies.Remove(dbMovie);

        _dbContext.Movies.Add(Movie);
        await _dbContext.SaveChangesAsync();
    }

    private async Task Cancel()
    {
        UploadTaskTokens.ForEach(ts => ts.Cancel());

        System.Windows.Application.Current.Dispatcher.Invoke(() => VideoProgress = 0);
        if (VideoProgressCompleted) await _awsStorageManager.DeleteFileAsync(Movie.VideoUrl);

        VideoImageProgress.Progress = 0;
        if (VideoImageProgressCompleted) await _storageManager.DeleteFileAsync(Movie.VideoImageUrl);

        TrailerProgress.Progress = 0;
        if (VideoProgressCompleted) await _storageManager.DeleteFileAsync(Movie.TrailerUrl);

        ImageProgress.Progress = 0;
        if (VideoProgressCompleted) await _storageManager.DeleteFileAsync(Movie.ImageUrl);

        SearchImageProgress.Progress = 0;
        if (VideoProgressCompleted) await _storageManager.DeleteFileAsync(Movie.SearchImageUrl);
    }

    private void OpenVideoDialog()
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "MP4 Files(*.mp4)|*.mp4|AVI Files(*.avi)|*.avi|MOV Files(*.mov)|*.mov";
        fileDialog.FilterIndex = 1;

        if (fileDialog.ShowDialog() is false) return;

        Movie.VideoUrl = fileDialog.FileName;
    }

    private void OpenVideoImageDialog()
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "PNG Files(*.png)|*.png|JPEG Files(*.jpeg)|*.jpeg|JPG Files(*.jpg)|*.jpg";
        fileDialog.FilterIndex = 3;

        if (fileDialog.ShowDialog() is false) return;

        Movie.VideoImageUrl = fileDialog.FileName;
    }

    private void OpenTrailerDialog()
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "MP4 Files(*.mp4)|*.mp4|AVI Files(*.avi)|*.avi|MOV Files(*.mov)|*.mov";
        fileDialog.FilterIndex = 1;

        if (fileDialog.ShowDialog() is false) return;

        Movie.TrailerUrl = fileDialog.FileName;
    }

    private void OpenImageDialog()
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "PNG Files(*.png)|*.png|JPEG Files(*.jpeg)|*.jpeg|JPG Files(*.jpg)|*.jpg";
        fileDialog.FilterIndex = 3;

        if (fileDialog.ShowDialog() is false) return;

        Movie.ImageUrl = fileDialog.FileName;
    }

    private void OpenSearchImageDialog()
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "PNG Files(*.png)|*.png|JPEG Files(*.jpeg)|*.jpeg|JPG Files(*.jpg)|*.jpg";
        fileDialog.FilterIndex = 3;

        if (fileDialog.ShowDialog() is false) return;

        Movie.SearchImageUrl = fileDialog.FileName;
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
