namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddSerialViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;
    private readonly IAWSStorageManager _awsStorageManager;

    public UploadSerialModel Serial { get; set; }
    public UploadSeasonModel Season { get; set; }
    public UploadEpisodeModel Episode { get; set; }
    public List<Producer> Producers { get; set; }

    public bool ProcessStarted { get; set; }
    public List<Task> UploadTasks { get; set; }
    public List<CancellationTokenSource> UploadTaskTokens { get; set; }

    public RelayCommand SaveCommand { get; set; }
    public RelayCommand CancelCommand { get; set; }

    public RelayCommand OpenVideoDialogCommand { get; set; }
    public RelayCommand OpenVideoImageDialogCommand { get; set; }
    public RelayCommand OpenTrailerDialogCommand { get; set; }
    public RelayCommand OpenImageDialogCommand { get; set; }
    public RelayCommand OpenSearchImageDialogCommand { get; set; }


    public AddSerialViewModel(AppDbContext dbContext, IStorageManager storageManager, IAWSStorageManager awsStorageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;
        _awsStorageManager = awsStorageManager;

        Producers = dbContext.Producers.ToList();

        Serial = new();
        Season = new();
        Episode = new();

        ProcessStarted = false;
        UploadTasks = new();
        UploadTaskTokens = new();

        SaveCommand = new RelayCommand(_ => Save(), _ => !ProcessStarted);
        CancelCommand = new RelayCommand(_ => Cancel(), _ => ProcessStarted);
        
        OpenVideoDialogCommand = new RelayCommand(_ => Episode.VideoUrl = FileDialogService.OpenVideoFile(), _ => !ProcessStarted);
        OpenVideoImageDialogCommand = new RelayCommand(_ => Episode.ImageUrl = FileDialogService.OpenImageFile(), _ => !ProcessStarted);
        OpenTrailerDialogCommand = new RelayCommand(_ => Serial.TrailerUrl = FileDialogService.OpenVideoFile(), _ => !ProcessStarted);
        OpenImageDialogCommand = new RelayCommand(_ => Serial.ImageUrl = FileDialogService.OpenImageFile(), _ => !ProcessStarted);
        OpenSearchImageDialogCommand = new RelayCommand(_ => Serial.SearchImageUrl = FileDialogService.OpenImageFile(), _ => !ProcessStarted);
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        Serial.Verify();
        Episode.Verify();

        if (Serial.HasErrors || Episode.HasErrors) return;

        ProcessStarted = true;

        var serial = Serial.Adapt<Serial>();
        var season = Season.Adapt<Season>();
        var episode = Episode.Adapt<Episode>();

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

        Episode.VideoUploadSuccess = false;
        Episode.ImageUploadSuccess = false;

        Serial.TrailerUploadSuccess = false;
        Serial.ImageUploadSuccess = false;
        Serial.SearchImageUploadSuccess = false;

        // Episode VideoUrl
        if (dbEpisode is null || dbEpisode is not null && dbEpisode.VideoUrl != Episode.VideoUrl)
        {
            var videoStream = new FileStream(Episode.VideoUrl, FileMode.Open, FileAccess.Read);
            var filename = string.Format("{0}-S01E01-video{1}", Path.GetFileNameWithoutExtension(Serial.Name).ToLower().Replace(' ', '-'), Path.GetExtension(Episode.VideoUrl));
            episode.VideoUrl = string.Format("Serials/{0}/Season 1/Episode 1/{1}", Serial.Name, filename);

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
            var filename = string.Format("{0}-S01E01-video-image{1}", Path.GetFileNameWithoutExtension(Serial.Name).ToLower().Replace(' ', '-'), Path.GetExtension(Episode.ImageUrl));
            episode.ImageUrl = string.Format("Serials/{0}/Season 1/Episode 1/{1}", Serial.Name, filename);

            Episode.ImageProgress = new BlobStorageUploadProgress(videoImageStream.Length);

            var imageToken = new CancellationTokenSource();
            var imageUploadTask = _storageManager.UploadFileAsync(videoImageStream, episode.ImageUrl, Episode.ImageProgress, imageToken.Token);

            UploadTasks.Add(imageUploadTask);
            UploadTaskTokens.Add(imageToken);

            imageUploadTask.ContinueWith(_ => Episode.ImageUploadSuccess = true);
        }
        else Episode.ImageUploadSuccess = true;

        // Serial TrailerUrl
        if (dbSerial is null || dbSerial is not null && dbSerial.TrailerUrl != Serial.TrailerUrl)
        {
            var trailerStream = new FileStream(Serial.TrailerUrl, FileMode.Open, FileAccess.Read);
            var filename = string.Format("{0}-trailer{1}", Path.GetFileNameWithoutExtension(Serial.Name).ToLower().Replace(' ', '-'), Path.GetExtension(Serial.TrailerUrl));
            serial.TrailerUrl = string.Format("Serials/{0}/{1}", Serial.Name, filename);

            Serial.TrailerProgress = new BlobStorageUploadProgress(trailerStream.Length);

            var trailerToken = new CancellationTokenSource();
            var trailerUploadTask = _storageManager.UploadFileAsync(trailerStream, serial.TrailerUrl, Serial.TrailerProgress, trailerToken.Token);

            UploadTasks.Add(trailerUploadTask);
            UploadTaskTokens.Add(trailerToken);

            trailerUploadTask.ContinueWith(_ => Serial.TrailerUploadSuccess = true);
        }
        else Serial.TrailerUploadSuccess = true;

        // Serial ImageUrl
        if (dbSerial is null || dbSerial is not null && dbSerial.ImageUrl != Serial.ImageUrl)
        {
            var imageStream = new FileStream(Serial.ImageUrl, FileMode.Open, FileAccess.Read);
            var filename = string.Format("{0}-image{1}", Path.GetFileNameWithoutExtension(Serial.Name).ToLower().Replace(' ', '-'), Path.GetExtension(Serial.ImageUrl));
            serial.ImageUrl = string.Format("Serials/{0}/{1}", Serial.Name, filename);

            Serial.ImageProgress = new BlobStorageUploadProgress(imageStream.Length);

            var imageToken = new CancellationTokenSource();
            var imageUploadTask = _storageManager.UploadFileAsync(imageStream, serial.ImageUrl, Serial.ImageProgress, imageToken.Token);

            UploadTasks.Add(imageUploadTask);
            UploadTaskTokens.Add(imageToken);

            imageUploadTask.ContinueWith(_ => Serial.ImageUploadSuccess = true);
        }
        else Serial.ImageUploadSuccess = true;

        // Serial SearchImageUrl
        if (dbSerial is null || dbSerial is not null && dbSerial.SearchImageUrl != Serial.SearchImageUrl)
        {
            var searchImageStream = new FileStream(Serial.SearchImageUrl, FileMode.Open, FileAccess.Read);
            var filename = string.Format("{0}-search-image{1}", Path.GetFileNameWithoutExtension(Serial.Name).ToLower().Replace(' ', '-'), Path.GetExtension(Serial.SearchImageUrl));
            serial.SearchImageUrl = string.Format("Serials/{0}/{1}", Serial.Name, filename);

            Serial.SearchImageProgress = new BlobStorageUploadProgress(searchImageStream.Length);

            var searchImageToken = new CancellationTokenSource();
            var searchImageUploadTask = _storageManager.UploadFileAsync(searchImageStream, serial.SearchImageUrl, Serial.SearchImageProgress, searchImageToken.Token);

            UploadTasks.Add(searchImageUploadTask);
            UploadTaskTokens.Add(searchImageToken);

            searchImageUploadTask.ContinueWith(_ => Serial.SearchImageUploadSuccess = true);
        }
        else Serial.SearchImageUploadSuccess = true;

        if (UploadTasks.Count > 0) await Task.WhenAll(UploadTasks);

        if (dbSerial is not null) _dbContext.Serials.Remove(dbSerial);

        _dbContext.Serials.Add(serial);
        await _dbContext.SaveChangesAsync();

        season.Number = 1;
        season.SerialName = Serial.Name;

        _dbContext.Seasons.Add(season);
        await _dbContext.SaveChangesAsync();

        episode.Number = 1;
        episode.SeasonId = season.Id;

        _dbContext.Episodes.Add(episode);
        await _dbContext.SaveChangesAsync();

        App.ServiceProvider.GetService<SerialViewModel>().Serials.Add(serial);
        App.ServiceProvider.GetService<SeasonViewModel>().Seasons.Add(season);
        App.ServiceProvider.GetService<EpisodeViewModel>().Episodes.Add(episode);

        ProcessStarted = false;
    }

    private async Task Cancel()
    {
        ProcessStarted = false;
        
        UploadTaskTokens.ForEach(ts => ts.Cancel());

        System.Windows.Application.Current.Dispatcher.Invoke(() => Episode.VideoProgress = 0);
        if (Episode.VideoUploadSuccess) await _awsStorageManager.DeleteFileAsync(Episode.VideoUrl);

        Episode.ImageProgress.Progress = 0;
        if (Episode.ImageUploadSuccess) await _storageManager.DeleteFileAsync(Episode.ImageUrl);

        Serial.TrailerProgress.Progress = 0;
        if (Serial.TrailerUploadSuccess) await _storageManager.DeleteFileAsync(Serial.TrailerUrl);

        Serial.ImageProgress.Progress = 0;
        if (Serial.ImageUploadSuccess) await _storageManager.DeleteFileAsync(Serial.ImageUrl);

        Serial.SearchImageProgress.Progress = 0;
        if (Serial.SearchImageUploadSuccess) await _storageManager.DeleteFileAsync(Serial.SearchImageUrl);
    }
}
