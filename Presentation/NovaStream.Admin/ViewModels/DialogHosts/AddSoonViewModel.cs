namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddSoonViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;

    public SoonViewModelContent Soon { get; set; }

    public bool IsEdit { get; set; }
    public bool ProcessStarted
    {
        get { return (bool)GetValue(ProcessStartedProperty); }
        set { SetValue(ProcessStartedProperty, value); }
    }
    public static readonly DependencyProperty ProcessStartedProperty =
        DependencyProperty.Register("ProcessStarted", typeof(bool), typeof(AddSoonViewModel));
    
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

        IsEdit = false;
        ProcessStarted = false;
        UploadTasks = new();
        UploadTaskTokens = new();

        SaveCommand = new RelayCommand(() => Save());
        CancelCommand = new RelayCommand(() => Cancel());

        OpenTrailerDialogCommand = new RelayCommand(() => Soon.TrailerUrl = FileDialogService.OpenVideoFile(Soon.TrailerUrl));
        OpenTrailerImageDialogCommand = new RelayCommand(() => Soon.TrailerImageUrl = FileDialogService.OpenImageFile(Soon.TrailerImageUrl));
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        try
        {
            Soon.Verify();

            if (Soon.HasErrors) return;

            var dbSoon = _dbContext.Soons.FirstOrDefault(s => s.Name == Soon.Name);

            if (!IsEdit && dbSoon is not null)
                Soon.AddError(nameof(Soon.Name), "Soon with this name already exists!");

            if (Soon.HasErrors) return;

            ProcessStarted = true;

            var soon = Soon.Adapt<Soon>();

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

            // Soon TrailerImageUrl
            if (dbSoon is null || dbSoon is not null && dbSoon.TrailerImageUrl != Soon.TrailerImageUrl)
            {
                var imageStream = new FileStream(Soon.TrailerImageUrl, FileMode.Open, FileAccess.Read);
                var filename = string.Format("{0}-trailer-image-{1}{2}", Path.GetFileNameWithoutExtension(Soon.Name).ToLower().Replace(' ', '-'), Random.Shared.Next(), Path.GetExtension(Soon.TrailerImageUrl));
                soon.TrailerImageUrl = string.Format("Soons/{0}/{1}", Soon.Name, filename);
                
                Soon.TrailerImageProgress = new BlobStorageUploadProgress(imageStream.Length);

                if (dbSoon is not null) _ = _storageManager.DeleteFileAsync(dbSoon.TrailerImageUrl);
                
                var imageToken = new CancellationTokenSource();
                var imageUploadTask = _storageManager.UploadFileAsync(imageStream, soon.TrailerImageUrl, Soon.TrailerImageProgress, imageToken.Token);

                UploadTasks.Add(imageUploadTask);
                UploadTaskTokens.Add(imageToken);

                imageUploadTask.ContinueWith(_ => Soon.TrailerImageUploadSuccess = true);
            }

            if (UploadTasks.Count > 0) await Task.WhenAll(UploadTasks);

            var soonViewModel = App.ServiceProvider.GetService<SoonViewModel>();
            
            if (dbSoon is not null)
            {
                var entity = soonViewModel.Soons.FirstOrDefault(s => s.Name == dbSoon.Name);
                _dbContext.Entry(entity).State = EntityState.Detached;

                var index = soonViewModel.Soons.IndexOf(entity);
                soonViewModel.Soons.RemoveAt(index);

                _dbContext.Soons.Update(soon);

                soonViewModel.Soons.Insert(index, soon);
            }
            else
            {
                _dbContext.Soons.Add(soon);
                soonViewModel.Soons.Add(soon);
            }

            await _dbContext.SaveChangesAsync();

            Soon.TrailerUploadSuccess = true;
            Soon.TrailerImageUploadSuccess = true;

            ProcessStarted = false;

            DialogHost.Close("RootDialog");

            await MessageBoxService.Show("Soon saved successfully!", MessageBoxType.Success);
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

        Soon.TrailerProgress.Progress = 0;
        Soon.TrailerImageProgress.Progress = 0;

        if (Soon.TrailerUploadSuccess) await _storageManager.DeleteFileAsync(Soon.TrailerUrl);
        if (Soon.TrailerImageUploadSuccess) await _storageManager.DeleteFileAsync(Soon.TrailerImageUrl);
    }
}
