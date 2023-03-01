namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddEpisodeViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;
    private readonly IAWSStorageManager _awsStorageManager;

    public EpisodeViewModelContent Episode { get; set; }

    public List<Serial> Serials
    {
        get { return (List<Serial>)GetValue(SerialsProperty); }
        set { SetValue(SerialsProperty, value); }
    }
    public static readonly DependencyProperty SerialsProperty =
        DependencyProperty.Register("Serials", typeof(List<Serial>), typeof(AddEpisodeViewModel));
    
    public List<Season> Seasons
    {
        get { return (List<Season>)GetValue(SeasonsProperty); }
        set { SetValue(SeasonsProperty, value); }
    }
    public static readonly DependencyProperty SeasonsProperty =
        DependencyProperty.Register("Seasons", typeof(List<Season>), typeof(AddEpisodeViewModel));

    public bool ProcessStarted
    {
        get { return (bool)GetValue(ProcessStartedProperty); }
        set { SetValue(ProcessStartedProperty, value); }
    }
    public static readonly DependencyProperty ProcessStartedProperty =
        DependencyProperty.Register("ProcessStarted", typeof(bool), typeof(AddEpisodeViewModel));

    public List<Task> UploadTasks { get; set; }
    public List<CancellationTokenSource> UploadTaskTokens { get; set; }

    public RelayCommand SaveCommand { get; set; }
    public RelayCommand CancelCommand { get; set; }

    public RelayCommand OpenVideoDialogCommand { get; set; }
    public RelayCommand OpenVideoImageDialogCommand { get; set; }

    public RelayCommand SelectedSerialChangedCommand { get; set; }
    public RelayCommand SelectedSeasonChangedCommand { get; set; }


    public AddEpisodeViewModel(AppDbContext dbContext, IStorageManager storageManager, IAWSStorageManager awsStorageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;
        _awsStorageManager = awsStorageManager;

        Episode = new();

        Serials = new List<Serial>(_dbContext.Serials);
        Seasons = new List<Season>();

        ProcessStarted = false;
        UploadTasks = new();
        UploadTaskTokens = new();

        SaveCommand = new RelayCommand(() => Save());
        CancelCommand = new RelayCommand(() => Cancel());

        OpenVideoDialogCommand = new RelayCommand(() => Episode.VideoUrl = FileDialogService.OpenVideoFile(Episode.VideoUrl));
        OpenVideoImageDialogCommand = new RelayCommand(() => Episode.ImageUrl = FileDialogService.OpenImageFile(Episode.ImageUrl));

        SelectedSerialChangedCommand = new RelayCommand(() => SelectedSerialChanged());
        SelectedSeasonChangedCommand = new RelayCommand(() => SelectedSeasonChanged());
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        try
        {
            Episode.Verify();
            Episode.Serial = Episode.Serial;
            Episode.Season = Episode.Season;

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

            if (UploadTasks.Count > 0) await Task.WhenAll(UploadTasks);

            if (dbEpisode is not null) _dbContext.Episodes.Remove(dbEpisode);

            _dbContext.Episodes.Add(episode);
            await _dbContext.SaveChangesAsync();

            App.ServiceProvider.GetService<EpisodeViewModel>()?.Episodes.Add(episode);

            Episode.VideoUploadSuccess = true;
            Episode.ImageUploadSuccess = true;

            ProcessStarted = false;

            DialogHost.Close("RootDialog");

            await MessageBoxService.Show($"Episode saved succesfully!", MessageBoxType.Success);
        }
        catch (OperationCanceledException) { return; }
        catch
        {
            if (!InternetService.CheckInternet())
                await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error);

            else
                await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);

            await Cancel();
        }
    }

    private async Task Cancel()
    {
        ProcessStarted = false;

        UploadTaskTokens.ForEach(ts => ts.Cancel());

        System.Windows.Application.Current.Dispatcher.Invoke(() => Episode.VideoProgress = 0);
        Episode.ImageProgress.Progress = 0;

        if (Episode.VideoUploadSuccess) await _awsStorageManager.DeleteFileAsync(Episode.VideoUrl);
        if (Episode.ImageUploadSuccess) await _storageManager.DeleteFileAsync(Episode.ImageUrl);
    }

    private void SelectedSerialChanged()
    {
        Seasons = _dbContext.Seasons.Where(s => s.SerialName == Episode.Serial.Name).ToList();

        Episode.Season = Seasons.FirstOrDefault();
    }

    private void SelectedSeasonChanged()
    {
        if (Episode.Season is null) Episode.Season = Seasons.FirstOrDefault();

        var episodes = _dbContext.Episodes.Where(e => e.SeasonId == Episode.Season.Id);

        Episode.Number = episodes.Count() == 0 ? 1 : episodes.Max(e => e.Number) + 1;
    }
}
