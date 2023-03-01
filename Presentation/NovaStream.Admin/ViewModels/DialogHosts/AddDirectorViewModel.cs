namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddDirectorViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;

    public DirectorViewModelContent Director { get; set; }

    public bool ProcessStarted
    {
        get { return (bool)GetValue(ProcessStartedProperty); }
        set { SetValue(ProcessStartedProperty, value); }
    }
    public static readonly DependencyProperty ProcessStartedProperty =
        DependencyProperty.Register("ProcessStarted", typeof(bool), typeof(AddDirectorViewModel));
    
    public List<Task> UploadTasks { get; set; }
    public List<CancellationTokenSource> UploadTaskTokens { get; set; }

    public RelayCommand SaveCommand { get; set; }
    public RelayCommand CancelCommand { get; set; }
    public RelayCommand OpenImageDialogCommand { get; set; }


    public AddDirectorViewModel(AppDbContext dbContext, IStorageManager storageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;

        Director = new();

        ProcessStarted = false;
        UploadTasks = new();
        UploadTaskTokens = new();

        SaveCommand = new RelayCommand(() => Save());
        CancelCommand = new RelayCommand(() => Cancel());

        OpenImageDialogCommand = new RelayCommand(() => Director.ImageUrl = FileDialogService.OpenImageFile(Director.ImageUrl));
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        try
        {
            Director.Verify();

            if (Director.HasErrors) return;

            ProcessStarted = true;

            var director = Director.Adapt<Director>();

            var dbDirector = _dbContext.Directors.FirstOrDefault(p => p.Id == Director.Id);

            UploadTasks.Clear();
            UploadTaskTokens.Clear();

            Director.ImageUploadSuccess = false;

            // Director ImageUrl
            if (dbDirector is null || dbDirector is not null && dbDirector.ImageUrl != Director.ImageUrl)
            {
                var imageStream = new FileStream(Director.ImageUrl, FileMode.Open, FileAccess.Read);
                var filename = string.Format("{0}-image-{1}{2}", $"{Director.Name} {Director.Surname}".Replace(' ', '-'), Random.Shared.Next(), Path.GetExtension(Director.ImageUrl));
                director.ImageUrl = string.Format("Images/Directors/{0}", filename);

                Director.ImageProgress = new BlobStorageUploadProgress(imageStream.Length);

                if (dbDirector is not null) _ = _storageManager.DeleteFileAsync(dbDirector.ImageUrl);

                var imageToken = new CancellationTokenSource();
                var imageUploadTask = _storageManager.UploadFileAsync(imageStream, director.ImageUrl, Director.ImageProgress, imageToken.Token);

                UploadTasks.Add(imageUploadTask);
                UploadTaskTokens.Add(imageToken);

                imageUploadTask.ContinueWith(_ => Director.ImageUploadSuccess = true);
            }

            if (UploadTasks.Count > 0) await Task.WhenAll(UploadTasks);

            var directorViewModel = App.ServiceProvider.GetService<DirectorViewModel>();

            if (dbDirector is not null)
            {
                var entity = directorViewModel.Directors.FirstOrDefault(p => p.Id == dbDirector.Id);
                _dbContext.Entry(entity).State = EntityState.Detached;

                var index = directorViewModel.Directors.IndexOf(entity);
                directorViewModel.Directors.RemoveAt(index);

                director.Id = dbDirector.Id;
                _dbContext.Directors.Update(director);

                directorViewModel.Directors.Insert(index, director);
            }
            else
            {
                _dbContext.Directors.Add(director);
                directorViewModel.Directors.Add(director);
            }

            await _dbContext.SaveChangesAsync();

            Director.ImageUploadSuccess = true;

            ProcessStarted = false;

            DialogHost.Close("RootDialog");

            await MessageBoxService.Show("Director saved succesfully!", MessageBoxType.Success);
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

        Director.ImageProgress.Progress = 0;

        if (Director.ImageUploadSuccess) await _storageManager.DeleteFileAsync(Director.ImageUrl);
    }
}
