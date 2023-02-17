namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddSoonViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;

    public UploadSoonModel Soon { get; set; }

    public bool ProcessStarted { get; set; }
    public List<Task> UploadTasks { get; set; }
    public List<CancellationTokenSource> UploadTaskTokens { get; set; }

    public RelayCommand SaveCommand { get; set; }
    public RelayCommand CancelCommand { get; set; }

    public RelayCommand OpenTrailerDialogCommand { get; set; }
    public RelayCommand OpenTrailerImageDialogCommand { get; set; }


    public AddSoonViewModel(AppDbContext dbContext, IStorageManager storageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;

        Soon = new();

        ProcessStarted = false;
        UploadTasks = new();
        UploadTaskTokens = new();

        SaveCommand = new RelayCommand(_ => Save(), _ => !ProcessStarted);
        CancelCommand = new RelayCommand(_ => Cancel(), _ => ProcessStarted);

        OpenTrailerDialogCommand = new RelayCommand(_ => Soon.TrailerUrl = FileDialogService.OpenVideoFile(), _ => !ProcessStarted);
        OpenTrailerImageDialogCommand = new RelayCommand(_ => Soon.TrailerImageUrl = FileDialogService.OpenImageFile(), _ => !ProcessStarted);
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        Soon.Verify();

        if (Soon.HasErrors) return;

        ProcessStarted = true;

        var soon = Soon.Adapt<Soon>();

        var dbSoon = _dbContext.Soons.FirstOrDefault(s => s.Name == Soon.Name);

        UploadTasks.Clear();
        UploadTaskTokens.Clear();

        Soon.TrailerUploadSuccess = false;
        Soon.TrailerImageUploadSuccess = false;

        // Soon TrailerUrl
        if (dbSoon is null || dbSoon is not null && dbSoon.TrailerUrl != Soon.TrailerUrl)
        {
            var trailerStream = new FileStream(Soon.TrailerUrl, FileMode.Open, FileAccess.Read);
            var filename = string.Format("{0}-trailer{1}", Path.GetFileNameWithoutExtension(Soon.Name).ToLower().Replace(' ', '-'), Path.GetExtension(Soon.TrailerUrl));
            soon.TrailerUrl = string.Format("Soons/{0}/{1}", Soon.Name, filename);

            Soon.TrailerProgress = new BlobStorageUploadProgress(trailerStream.Length);

            var trailerToken = new CancellationTokenSource();
            var trailerUploadTask = _storageManager.UploadFileAsync(trailerStream, soon.TrailerUrl, Soon.TrailerProgress, trailerToken.Token);

            UploadTasks.Add(trailerUploadTask);
            UploadTaskTokens.Add(trailerToken);

            trailerUploadTask.ContinueWith(_ => Soon.TrailerUploadSuccess = true);
        }
        else Soon.TrailerUploadSuccess = true;

        // Soon TrailerImageUrl
        if (dbSoon is null || dbSoon is not null && dbSoon.TrailerImageUrl != Soon.TrailerImageUrl)
        {
            var imageStream = new FileStream(Soon.TrailerImageUrl, FileMode.Open, FileAccess.Read);
            var filename = string.Format("{0}-trailer-image{1}", Path.GetFileNameWithoutExtension(Soon.Name).ToLower().Replace(' ', '-'), Path.GetExtension(Soon.TrailerImageUrl));
            Soon.TrailerImageUrl = string.Format("Soons/{0}/{1}", Soon.Name, filename);

            Soon.TrailerImageProgress = new BlobStorageUploadProgress(imageStream.Length);

            var imageToken = new CancellationTokenSource();
            var imageUploadTask = _storageManager.UploadFileAsync(imageStream, soon.TrailerImageUrl, Soon.TrailerImageProgress, imageToken.Token);

            UploadTasks.Add(imageUploadTask);
            UploadTaskTokens.Add(imageToken);

            imageUploadTask.ContinueWith(_ => Soon.TrailerImageUploadSuccess = true);
        }
        else Soon.TrailerImageUploadSuccess = true;

        if (UploadTasks.Count > 0) await Task.WhenAll(UploadTasks);

        if (dbSoon is not null) _dbContext.Soons.Remove(dbSoon);

        _dbContext.Soons.Add(soon);
        await _dbContext.SaveChangesAsync();

        App.ServiceProvider.GetService<SoonViewModel>().Soons.Add(soon);

        ProcessStarted = false;
    }

    private async Task Cancel()
    {
        ProcessStarted = false;

        UploadTaskTokens.ForEach(ts => ts.Cancel());

        Soon.TrailerProgress.Progress = 0;
        if (Soon.TrailerUploadSuccess) await _storageManager.DeleteFileAsync(Soon.TrailerUrl);

        Soon.TrailerImageProgress.Progress = 0;
        if (Soon.TrailerImageUploadSuccess) await _storageManager.DeleteFileAsync(Soon.TrailerImageUrl);
    }
}
