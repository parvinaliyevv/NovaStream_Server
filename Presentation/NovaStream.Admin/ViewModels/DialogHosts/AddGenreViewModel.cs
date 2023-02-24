namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddGenreViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;

    public UploadGenreModel Genre { get; set; }

    public bool IsEdit { get; set; }
    public bool ProcessStarted { get; set; }
    public List<Task> UploadTasks { get; set; }
    public List<CancellationTokenSource> UploadTaskTokens { get; set; }

    public RelayCommand SaveCommand { get; set; }
    public RelayCommand CancelCommand { get; set; }
    public RelayCommand OpenImageDialogCommand { get; set; }


    public AddGenreViewModel(AppDbContext dbContext, IStorageManager storageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;

        Genre = new();

        IsEdit = false;
        ProcessStarted = false;
        UploadTasks = new();
        UploadTaskTokens = new();

        SaveCommand = new RelayCommand(_ => Save(), _ => !ProcessStarted);
        CancelCommand = new RelayCommand(_ => Cancel(), _ => ProcessStarted);

        OpenImageDialogCommand = new RelayCommand(_ => Genre.ImageUrl = FileDialogService.OpenImageFile(), _ => !ProcessStarted);
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        try
        {
            Genre.Verify();

            if (Genre.HasErrors) return;

            var dbGenre = _dbContext.Genres.FirstOrDefault(g => g.Name == Genre.Name);

            if (!IsEdit && dbGenre is not null)
                Genre.AddError(nameof(Genre.Name), "Genre with this name already exists!");

            if (Genre.HasErrors) return;

            ProcessStarted = true;

            var genre = Genre.Adapt<Genre>();

            UploadTasks.Clear();
            UploadTaskTokens.Clear();

            Genre.ImageUploadSuccess = false;

            // Genre ImageUrl
            if (dbGenre is null || dbGenre is not null && dbGenre.ImageUrl != Genre.ImageUrl)
            {
                var imageStream = new FileStream(Genre.ImageUrl, FileMode.Open, FileAccess.Read);
                var filename = string.Format("{0}-image{1}", Genre.Name.ToLower().Replace(' ', '-'), Path.GetExtension(Genre.ImageUrl));
                genre.ImageUrl = string.Format("Images/Genres/{0}", filename);

                Genre.ImageProgress = new BlobStorageUploadProgress(imageStream.Length);

                var imageToken = new CancellationTokenSource();
                var imageUploadTask = _storageManager.UploadFileAsync(imageStream, genre.ImageUrl, Genre.ImageProgress, imageToken.Token);

                UploadTasks.Add(imageUploadTask);
                UploadTaskTokens.Add(imageToken);

                imageUploadTask.ContinueWith(_ => Genre.ImageUploadSuccess = true);
            }
            else Genre.ImageUploadSuccess = true;

            if (UploadTasks.Count > 0) await Task.WhenAll(UploadTasks);

            var genreViewModel = App.ServiceProvider.GetService<GenreViewModel>();

            if (dbGenre is not null)
            {
                var entity = genreViewModel.Genres.FirstOrDefault(g => g.Id == dbGenre.Id);
                _dbContext.Entry(entity).State = EntityState.Detached;

                var index = genreViewModel.Genres.IndexOf(entity);
                genreViewModel.Genres.RemoveAt(index);

                genre.Id = dbGenre.Id;
                _dbContext.Genres.Update(genre);

                genreViewModel.Genres.Insert(index, genre);
            }
            else
            {
                _dbContext.Genres.Add(genre);
                genreViewModel.Genres.Add(genre);
            }

            await _dbContext.SaveChangesAsync();

            ProcessStarted = false;

            DialogHost.Close("RootDialog");

            await MessageBoxService.Show("Genre saved successfully!", MessageBoxType.Success);
        }
        catch (Exception ex)
        {
            _ = Cancel();

            await MessageBoxService.Show(ex.Message, MessageBoxType.Error);
        }
    }

    private async Task Cancel()
    {
        ProcessStarted = false;

        UploadTaskTokens.ForEach(ts => ts.Cancel());

        Genre.ImageProgress.Progress = 0;
        if (Genre.ImageUploadSuccess) await _storageManager.DeleteFileAsync(Genre.ImageUrl);
    }
}
