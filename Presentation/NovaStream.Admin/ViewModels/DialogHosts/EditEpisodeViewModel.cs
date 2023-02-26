namespace NovaStream.Admin.ViewModels.DialogHosts;

public class EditEpisodeViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;
    private readonly IAWSStorageManager _awsStorageManager;

    public UploadEpisodeModel Episode { get; set; }

    public bool ProcessStarted
    {
        get { return (bool)GetValue(ProcessStartedProperty); }
        set { SetValue(ProcessStartedProperty, value); }
    }
    public static readonly DependencyProperty ProcessStartedProperty =
        DependencyProperty.Register("ProcessStarted", typeof(bool), typeof(EditEpisodeViewModel));

    public List<Task> UploadTasks { get; set; }
    public List<CancellationTokenSource> UploadTaskTokens { get; set; }

    public RelayCommand SaveCommand { get; set; }
    public RelayCommand CancelCommand { get; set; }

    public RelayCommand OpenVideoDialogCommand { get; set; }
    public RelayCommand OpenVideoImageDialogCommand { get; set; }


    public EditEpisodeViewModel(AppDbContext dbContext, IStorageManager storageManager, IAWSStorageManager awsStorageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;
        _awsStorageManager = awsStorageManager;

        Episode = new();

        ProcessStarted = false;
        UploadTasks = new();
        UploadTaskTokens = new();

        SaveCommand = new RelayCommand(_ => Save(), _ => !ProcessStarted);
        CancelCommand = new RelayCommand(_ => Cancel(), _ => ProcessStarted);

        OpenVideoDialogCommand = new RelayCommand(_ => Episode.VideoUrl = FileDialogService.OpenVideoFile(Episode.VideoUrl), _ => !ProcessStarted);
        OpenVideoImageDialogCommand = new RelayCommand(_ => Episode.ImageUrl = FileDialogService.OpenImageFile(Episode.ImageUrl), _ => !ProcessStarted);
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        try
        {
            Episode.Verify();

            if (Episode.HasErrors) return;

            ProcessStarted = true;

            var episode = Episode.Adapt<Episode>();

            var dbEpisode = _dbContext.Episodes.FirstOrDefault(e => e.SeasonId == Episode.Season.Id && e.Number == episode.Number);

            UploadTasks.Clear();
            UploadTaskTokens.Clear();

            Episode.VideoUploadSuccess = false;
            Episode.ImageUploadSuccess = false;

            // Episode VideoUrl
            if (dbEpisode is null || dbEpisode is not null && dbEpisode.VideoUrl != Episode.VideoUrl)
            {
                var videoStream = new FileStream(Episode.VideoUrl, FileMode.Open, FileAccess.Read);
                var filename = string.Format("{0}-S{1:00}E{2:00}-video{3}", Path.GetFileNameWithoutExtension(Episode.Serial.Name).ToLower().Replace(' ', '-'), Episode.Season.Number, Episode.Number, Path.GetExtension(Episode.VideoUrl));
                episode.VideoUrl = string.Format("Serials/{0}/Season {1}/Episode {2}/{3}", Episode.Serial.Name, Episode.Season.Number, Episode.Number, filename);

                var videoToken = new CancellationTokenSource();
                var videoUploadTask = _awsStorageManager.UploadFileAsync(videoStream, episode.VideoUrl, Episode.VideoProgressEvent, videoToken.Token);

                UploadTasks.Add(videoUploadTask);
                UploadTaskTokens.Add(videoToken);

                videoUploadTask.ContinueWith(_ => Episode.VideoUploadSuccess = true);
            }
            else Episode.VideoUploadSuccess = true;

            // Episode ImageUrl
            if (dbEpisode is null || dbEpisode is not null && dbEpisode.ImageUrl != Episode.ImageUrl)
            {
                var videoImageStream = new FileStream(Episode.ImageUrl, FileMode.Open, FileAccess.Read);
                var filename = string.Format("{0}-S{1:00}E{2:00}-video-image-{3}{4}", Path.GetFileNameWithoutExtension(Episode.Serial.Name).ToLower().Replace(' ', '-'), Episode.Season.Number, Episode.Number, Random.Shared.Next(), Path.GetExtension(Episode.ImageUrl));
                episode.ImageUrl = string.Format("Serials/{0}/Season {1}/Episode {2}/{3}", Episode.Serial.Name, Episode.Season.Number, Episode.Number, filename);

                Episode.ImageProgress = new BlobStorageUploadProgress(videoImageStream.Length);
                
                if (dbEpisode is not null) _ = _storageManager.DeleteFileAsync(dbEpisode.ImageUrl);

                var imageToken = new CancellationTokenSource();
                var imageUploadTask = _storageManager.UploadFileAsync(videoImageStream, episode.ImageUrl, Episode.ImageProgress, imageToken.Token);

                UploadTasks.Add(imageUploadTask);
                UploadTaskTokens.Add(imageToken);

                imageUploadTask.ContinueWith(_ => Episode.ImageUploadSuccess = true);
            }
            else Episode.ImageUploadSuccess = true;

            if (UploadTasks.Count > 0) await Task.WhenAll(UploadTasks);

            var episodeViewModel = App.ServiceProvider.GetService<EpisodeViewModel>();

            var entity = episodeViewModel.Episodes.FirstOrDefault(e => e.Id == dbEpisode.Id);
            _dbContext.Entry(entity).State = EntityState.Detached;

            var index = episodeViewModel.Episodes.IndexOf(entity);
            episodeViewModel.Episodes.RemoveAt(index);

            episode.Id = dbEpisode.Id;
            _dbContext.Episodes.Update(episode);

            episodeViewModel.Episodes.Insert(index, episode);

            await _dbContext.SaveChangesAsync();

            ProcessStarted = false;

            DialogHost.Close("RootDialog");

            await MessageBoxService.Show($"Episode saved succesfully!", MessageBoxType.Success);
        }
        catch (Exception ex)
        {
            await MessageBoxService.Show(ex.Message, MessageBoxType.Error);

            await Cancel();
        }
    }

    private async Task Cancel()
    {
        ProcessStarted = false;

        UploadTaskTokens.ForEach(ts => ts.Cancel());

        System.Windows.Application.Current.Dispatcher.Invoke(() => Episode.VideoProgress = 0);
        if (Episode.VideoUploadSuccess) await _awsStorageManager.DeleteFileAsync(Episode.VideoUrl);

        Episode.ImageProgress.Progress = 0;
        if (Episode.ImageUploadSuccess) await _storageManager.DeleteFileAsync(Episode.ImageUrl);
    }
}
