namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddSerialViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;
    private readonly IAWSStorageManager _awsStorageManager;

    public SerialViewModelContent Serial { get; set; }
    public SeasonViewModelContent Season { get; set; }
    public EpisodeViewModelContent Episode { get; set; }
    public List<Director> Directors { get; set; }

    public bool IsEdit { get; set; }
    public bool ProcessStarted
    {
        get { return (bool)GetValue(ProcessStartedProperty); }
        set { SetValue(ProcessStartedProperty, value); }
    }
    public static readonly DependencyProperty ProcessStartedProperty =
        DependencyProperty.Register("ProcessStarted", typeof(bool), typeof(AddSerialViewModel));
    
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

        Directors = dbContext.Directors.ToList();

        Serial = new();
        Season = new();
        Episode = new();

        IsEdit = false;
        ProcessStarted = false;
        UploadTasks = new();
        UploadTaskTokens = new();

        SaveCommand = new RelayCommand(() => Save());
        CancelCommand = new RelayCommand(() => Cancel());
        
        OpenVideoDialogCommand = new RelayCommand(() => Episode.VideoUrl = FileDialogService.OpenVideoFile(Episode.VideoUrl));
        OpenVideoImageDialogCommand = new RelayCommand(() => Episode.ImageUrl = FileDialogService.OpenImageFile(Episode.ImageUrl));
        OpenTrailerDialogCommand = new RelayCommand(() => Serial.TrailerUrl = FileDialogService.OpenVideoFile(Serial.TrailerUrl));
        OpenImageDialogCommand = new RelayCommand(() => Serial.ImageUrl = FileDialogService.OpenImageFile(Serial.ImageUrl));
        OpenSearchImageDialogCommand = new RelayCommand(() => Serial.SearchImageUrl = FileDialogService.OpenImageFile(Serial.SearchImageUrl));
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        try
        {
            Serial.Verify();
            Episode.Verify();

            if (Serial.HasErrors) return;

            var dbSerial = _dbContext.Serials.AsNoTracking().FirstOrDefault(s => s.Name == Serial.Name);

            if (!IsEdit && dbSerial is not null)
                Serial.AddError(nameof(Serial.Name), "Serial with this name already exists!");

            if (Serial.HasErrors || Episode.HasErrors) return;

            ProcessStarted = true;

            var serial = Serial.Adapt<Serial>();
            var season = Season.Adapt<Season>();
            var episode = Episode.Adapt<Episode>();

            Season? dbSeason = null;
            Episode? dbEpisode = null;

            if (dbSerial is not null)
            {
                dbSeason = _dbContext.Seasons.AsNoTracking().FirstOrDefault(s => s.SerialName == dbSerial.Name && s.Number == 1);

                if (dbSeason is not null)
                    dbEpisode = _dbContext.Episodes.AsNoTracking().FirstOrDefault(e => e.SeasonId == dbSeason.Id && e.Number == 1);
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

            // Episode ImageUrl
            if (dbEpisode is null || dbEpisode is not null && dbEpisode.ImageUrl != Episode.ImageUrl)
            {
                var videoImageStream = new FileStream(Episode.ImageUrl, FileMode.Open, FileAccess.Read);
                var filename = string.Format("{0}-S01E01-video-image-{1}{2}", Path.GetFileNameWithoutExtension(Serial.Name).ToLower().Replace(' ', '-'), Random.Shared.Next(), Path.GetExtension(Episode.ImageUrl));
                episode.ImageUrl = string.Format("Serials/{0}/Season 1/Episode 1/{1}", Serial.Name, filename);

                Episode.ImageProgress = new BlobStorageUploadProgress(videoImageStream.Length);

                if (dbEpisode is not null) _ = _storageManager.DeleteFileAsync(dbEpisode.ImageUrl);

                var imageToken = new CancellationTokenSource();
                var imageUploadTask = _storageManager.UploadFileAsync(videoImageStream, episode.ImageUrl, Episode.ImageProgress, imageToken.Token);

                UploadTasks.Add(imageUploadTask);
                UploadTaskTokens.Add(imageToken);

                imageUploadTask.ContinueWith(_ => Episode.ImageUploadSuccess = true);
            }

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

            // Serial ImageUrl
            if (dbSerial is null || dbSerial is not null && dbSerial.ImageUrl != Serial.ImageUrl)
            {
                var imageStream = new FileStream(Serial.ImageUrl, FileMode.Open, FileAccess.Read);
                var filename = string.Format("{0}-image-{1}{2}", Path.GetFileNameWithoutExtension(Serial.Name).ToLower().Replace(' ', '-'), Random.Shared.Next(), Path.GetExtension(Serial.ImageUrl));
                serial.ImageUrl = string.Format("Serials/{0}/{1}", Serial.Name, filename);

                Serial.ImageProgress = new BlobStorageUploadProgress(imageStream.Length);

                if (dbSerial is not null) _ = _storageManager.DeleteFileAsync(dbSerial.ImageUrl);

                var imageToken = new CancellationTokenSource();
                var imageUploadTask = _storageManager.UploadFileAsync(imageStream, serial.ImageUrl, Serial.ImageProgress, imageToken.Token);

                UploadTasks.Add(imageUploadTask);
                UploadTaskTokens.Add(imageToken);

                imageUploadTask.ContinueWith(_ => Serial.ImageUploadSuccess = true);
            }

            // Serial SearchImageUrl
            if (dbSerial is null || dbSerial is not null && dbSerial.SearchImageUrl != Serial.SearchImageUrl)
            {
                var searchImageStream = new FileStream(Serial.SearchImageUrl, FileMode.Open, FileAccess.Read);
                var filename = string.Format("{0}-search-image-{1}{2}", Path.GetFileNameWithoutExtension(Serial.Name).ToLower().Replace(' ', '-'), Random.Shared.Next(), Path.GetExtension(Serial.SearchImageUrl));
                serial.SearchImageUrl = string.Format("Serials/{0}/{1}", Serial.Name, filename);

                Serial.SearchImageProgress = new BlobStorageUploadProgress(searchImageStream.Length);

                if (dbSerial is not null) _ = _storageManager.DeleteFileAsync(dbSerial.SearchImageUrl);

                var searchImageToken = new CancellationTokenSource();
                var searchImageUploadTask = _storageManager.UploadFileAsync(searchImageStream, serial.SearchImageUrl, Serial.SearchImageProgress, searchImageToken.Token);

                UploadTasks.Add(searchImageUploadTask);
                UploadTaskTokens.Add(searchImageToken);

                searchImageUploadTask.ContinueWith(_ => Serial.SearchImageUploadSuccess = true);
            }

            if (UploadTasks.Count > 0) await Task.WhenAll(UploadTasks);

            if (!IsEdit)
            {
                var serialViewModel = App.ServiceProvider.GetService<SerialViewModel>();

                _dbContext.Serials.Add(serial);
                serialViewModel.Serials.Add(serial);

                await _dbContext.SaveChangesAsync();

                season.Number = 1;
                season.SerialName = serial.Name;

                if (SeasonViewModel.isCreated)
                {
                    var seasonViewModel = App.ServiceProvider.GetService<SeasonViewModel>();
                    seasonViewModel.Seasons.Add(season);
                }

                _dbContext.Seasons.Add(season);

                await _dbContext.SaveChangesAsync();

                episode.Number = 1;
                episode.Season = season;

                if (EpisodeViewModel.isCreated)
                {
                    var episodeViewModel = App.ServiceProvider.GetService<EpisodeViewModel>();
                    episodeViewModel.Episodes.Add(episode);
                }

                _dbContext.Episodes.Add(episode);

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                var serialViewModel = App.ServiceProvider.GetService<SerialViewModel>();

                var viewSerial = serialViewModel.Serials.FirstOrDefault(s => s.Name == dbSerial.Name);
                _dbContext.Entry(viewSerial).State = EntityState.Detached;

                var index = serialViewModel.Serials.IndexOf(viewSerial);
                serialViewModel.Serials.RemoveAt(index);

                _dbContext.Serials.Update(serial);

                serialViewModel.Serials.Insert(index, serial);

                await _dbContext.SaveChangesAsync();

                episode.Number = 1;
                episode.Season = dbSeason;

                if (dbEpisode is not null)
                {
                    episode.Id = dbEpisode.Id;

                    if (EpisodeViewModel.isCreated)
                    {
                        var episodeViewModel = App.ServiceProvider.GetService<EpisodeViewModel>();

                        var viewEpisode = episodeViewModel.Episodes.FirstOrDefault(e => e.Id == dbEpisode.Id);
                        _dbContext.Entry(viewEpisode).State = EntityState.Detached;

                        index = episodeViewModel.Episodes.IndexOf(viewEpisode);
                        episodeViewModel.Episodes.RemoveAt(index);

                        episodeViewModel.Episodes.Insert(index, episode);
                    }

                    _dbContext.Episodes.Update(episode);
                }
                else _dbContext.Episodes.Add(episode);

                await _dbContext.SaveChangesAsync();
            }

            Episode.VideoUploadSuccess = true;
            Episode.ImageUploadSuccess = true;
            Serial.TrailerUploadSuccess = true;
            Serial.ImageUploadSuccess = true;
            Serial.SearchImageUploadSuccess = true;

            ProcessStarted = false;

            DialogHost.Close("RootDialog");

            await MessageBoxService.Show("Serial saved successfully!", MessageBoxType.Success);
        }
        catch (OperationCanceledException) { return; }
        catch
        {
            if (!InternetService.CheckInternet())
                await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error);

            else
                await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);

            await Cancel();

            var dbSerial = _dbContext.Serials.FirstOrDefault(s => s.Name == Serial.Name);

            if (dbSerial is not null) _dbContext.Serials.Remove(dbSerial);
        }
    }

    private async Task Cancel()
    {
        ProcessStarted = false;
        
        UploadTaskTokens.ForEach(ts => ts.Cancel());

        System.Windows.Application.Current.Dispatcher.Invoke(() => Episode.VideoProgress = 0);
        Episode.ImageProgress.Progress = 0;
        Serial.TrailerProgress.Progress = 0;
        Serial.ImageProgress.Progress = 0;
        Serial.SearchImageProgress.Progress = 0;
        
        if (Episode.VideoUploadSuccess) await _awsStorageManager.DeleteFileAsync(Episode.VideoUrl);
        if (Episode.ImageUploadSuccess) await _storageManager.DeleteFileAsync(Episode.ImageUrl);
        if (Serial.TrailerUploadSuccess) await _storageManager.DeleteFileAsync(Serial.TrailerUrl);
        if (Serial.ImageUploadSuccess) await _storageManager.DeleteFileAsync(Serial.ImageUrl);
        if (Serial.SearchImageUploadSuccess) await _storageManager.DeleteFileAsync(Serial.SearchImageUrl);
    }
}
