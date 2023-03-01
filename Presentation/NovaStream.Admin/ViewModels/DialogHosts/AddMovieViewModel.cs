namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddMovieViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;
    private readonly IAWSStorageManager _awsStorageManager;

    public MovieViewModelContent Movie { get; set; }
    public List<Director> Directors { get; set; }

    public bool IsEdit { get; set; }
    public bool ProcessStarted
    {
        get { return (bool)GetValue(ProcessStartedProperty); }
        set { SetValue(ProcessStartedProperty, value); }
    }
    public static readonly DependencyProperty ProcessStartedProperty =
        DependencyProperty.Register("ProcessStarted", typeof(bool), typeof(AddMovieViewModel));
    
    public List<Task> UploadTasks { get; set; }
    public List<CancellationTokenSource> UploadTaskTokens { get; set; }

    public RelayCommand SaveCommand { get; set; }
    public RelayCommand CancelCommand { get; set; }

    public RelayCommand OpenVideoDialogCommand { get; set; }
    public RelayCommand OpenVideoImageDialogCommand { get; set; }
    public RelayCommand OpenTrailerDialogCommand { get; set; }
    public RelayCommand OpenImageDialogCommand { get; set; }
    public RelayCommand OpenSearchImageDialogCommand { get; set; }


    public AddMovieViewModel(AppDbContext dbContext, IStorageManager storageManager, IAWSStorageManager awsStorageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;
        _awsStorageManager = awsStorageManager;

        Directors = dbContext.Directors.ToList();

        Movie = new();

        IsEdit = false;
        ProcessStarted = false;
        UploadTasks = new();
        UploadTaskTokens = new();

        SaveCommand = new RelayCommand(() => Save());
        CancelCommand = new RelayCommand(() => Cancel());

        OpenVideoDialogCommand = new RelayCommand(() => Movie.VideoUrl = FileDialogService.OpenVideoFile(Movie.VideoUrl));
        OpenVideoImageDialogCommand = new RelayCommand(() => Movie.VideoImageUrl = FileDialogService.OpenImageFile(Movie.VideoImageUrl));
        OpenTrailerDialogCommand = new RelayCommand(() => Movie.TrailerUrl = FileDialogService.OpenVideoFile(Movie.TrailerUrl));
        OpenImageDialogCommand = new RelayCommand(() => Movie.ImageUrl = FileDialogService.OpenImageFile(Movie.ImageUrl));
        OpenSearchImageDialogCommand = new RelayCommand(() => Movie.SearchImageUrl = FileDialogService.OpenImageFile(Movie.SearchImageUrl));
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        try
        {
            Movie.Verify();

            if (Movie.HasErrors) return;

            var dbMovie = _dbContext.Movies.FirstOrDefault(m => m.Name == Movie.Name);

            if (!IsEdit && dbMovie is not null)
                Movie.AddError(nameof(Movie.Name), "Movie with this name already exists!");

            if (Movie.HasErrors) return;

            ProcessStarted = true;

            var movie = Movie.Adapt<Movie>();

            UploadTasks.Clear();
            UploadTaskTokens.Clear();

            Movie.VideoUploadSuccess = false;
            Movie.VideoImageUploadSuccess = false;
            Movie.TrailerUploadSuccess = false;
            Movie.ImageUploadSuccess = false;
            Movie.SearchImageUploadSuccess = false;

            // Movie VideoUrl
            if (dbMovie is null || dbMovie is not null && dbMovie.VideoUrl != Movie.VideoUrl)
            {
                var videoStream = new FileStream(Movie.VideoUrl, FileMode.Open, FileAccess.Read);
                var filename = string.Format("{0}-video{1}", Path.GetFileNameWithoutExtension(Movie.Name).ToLower().Replace(' ', '-'), Path.GetExtension(Movie.VideoUrl));
                movie.VideoUrl = string.Format("Movies/{0}/{1}", Movie.Name, filename);

                var videoToken = new CancellationTokenSource();
                var videoUploadTask = _awsStorageManager.UploadFileAsync(videoStream, movie.VideoUrl, Movie.VideoProgressEvent, videoToken.Token);

                UploadTasks.Add(videoUploadTask);
                UploadTaskTokens.Add(videoToken);

                videoUploadTask.ContinueWith(_ => Movie.VideoUploadSuccess = true);
            }

            // Movie VideoImageUrl
            if (dbMovie is null || dbMovie is not null && dbMovie.VideoImageUrl != Movie.VideoImageUrl)
            {
                var videoImageStream = new FileStream(Movie.VideoImageUrl, FileMode.Open, FileAccess.Read);
                var filename = string.Format("{0}-video-image-{1}{2}", Path.GetFileNameWithoutExtension(Movie.Name).ToLower().Replace(' ', '-'), Random.Shared.Next(), Path.GetExtension(Movie.VideoImageUrl));
                movie.VideoImageUrl = string.Format("Movies/{0}/{1}", Movie.Name, filename);

                Movie.VideoImageProgress = new BlobStorageUploadProgress(videoImageStream.Length);
                
                if (dbMovie is not null) _ = _storageManager.DeleteFileAsync(dbMovie.VideoImageUrl);

                var videoImageToken = new CancellationTokenSource();
                var videoImageUploadTask = _storageManager.UploadFileAsync(videoImageStream, movie.VideoImageUrl, Movie.VideoImageProgress, videoImageToken.Token);

                UploadTasks.Add(videoImageUploadTask);
                UploadTaskTokens.Add(videoImageToken);

                videoImageUploadTask.ContinueWith(_ => Movie.VideoImageUploadSuccess = true);
            }

            // Movie TrailerUrl
            if (dbMovie is null || dbMovie is not null && dbMovie.TrailerUrl != Movie.TrailerUrl)
            {
                var trailerStream = new FileStream(Movie.TrailerUrl, FileMode.Open, FileAccess.Read);
                var filename = string.Format("{0}-trailer{1}", Path.GetFileNameWithoutExtension(Movie.Name).ToLower().Replace(' ', '-'), Path.GetExtension(Movie.TrailerUrl));
                movie.TrailerUrl = string.Format("Movies/{0}/{1}", Movie.Name, filename);

                Movie.TrailerProgress = new BlobStorageUploadProgress(trailerStream.Length);

                var trailerToken = new CancellationTokenSource();
                var trailerUploadTask = _storageManager.UploadFileAsync(trailerStream, movie.TrailerUrl, Movie.TrailerProgress, trailerToken.Token);

                UploadTasks.Add(trailerUploadTask);
                UploadTaskTokens.Add(trailerToken);

                trailerUploadTask.ContinueWith(_ => Movie.TrailerUploadSuccess = true);
            }

            // Movie ImageUrl
            if (dbMovie is null || dbMovie is not null && dbMovie.ImageUrl != Movie.ImageUrl)
            {
                var imageStream = new FileStream(Movie.ImageUrl, FileMode.Open, FileAccess.Read);
                var filename = string.Format("{0}-image-{1}{2}", Path.GetFileNameWithoutExtension(Movie.Name).ToLower().Replace(' ', '-'), Random.Shared.Next(), Path.GetExtension(Movie.ImageUrl));
                movie.ImageUrl = string.Format("Movies/{0}/{1}", Movie.Name, filename);

                Movie.ImageProgress = new BlobStorageUploadProgress(imageStream.Length);

                if (dbMovie is not null) _ = _storageManager.DeleteFileAsync(dbMovie.ImageUrl);

                var imageToken = new CancellationTokenSource();
                var imageUploadTask = _storageManager.UploadFileAsync(imageStream, movie.ImageUrl, Movie.ImageProgress, imageToken.Token);

                UploadTasks.Add(imageUploadTask);
                UploadTaskTokens.Add(imageToken);

                imageUploadTask.ContinueWith(_ => Movie.ImageUploadSuccess = true);
            }

            // Movie SearchImageUrl
            if (dbMovie is null || dbMovie is not null && dbMovie.SearchImageUrl != Movie.SearchImageUrl)
            {
                var searchImageStream = new FileStream(Movie.SearchImageUrl, FileMode.Open, FileAccess.Read);
                var filename = string.Format("{0}-search-image-{1}{2}", Path.GetFileNameWithoutExtension(Movie.Name).ToLower().Replace(' ', '-'), Random.Shared.Next(), Path.GetExtension(Movie.SearchImageUrl));
                movie.SearchImageUrl = string.Format("Movies/{0}/{1}", Movie.Name, filename);

                Movie.SearchImageProgress = new BlobStorageUploadProgress(searchImageStream.Length);

                if (dbMovie is not null) _ = _storageManager.DeleteFileAsync(dbMovie.SearchImageUrl);

                var searchImageToken = new CancellationTokenSource();
                var searchImageUploadTask = _storageManager.UploadFileAsync(searchImageStream, movie.SearchImageUrl, Movie.SearchImageProgress, searchImageToken.Token);

                UploadTasks.Add(searchImageUploadTask);
                UploadTaskTokens.Add(searchImageToken);

                searchImageUploadTask.ContinueWith(_ => Movie.SearchImageUploadSuccess = true);
            }

            if (UploadTasks.Count > 0) await Task.WhenAll(UploadTasks);

            var movieViewModel = App.ServiceProvider.GetService<MovieViewModel>();

            if (dbMovie is not null)
            {
                var entity = movieViewModel.Movies.FirstOrDefault(m => m.Name == dbMovie.Name);
                _dbContext.Entry(entity).State = EntityState.Detached;

                var index = movieViewModel.Movies.IndexOf(entity);
                movieViewModel.Movies.RemoveAt(index);

                _dbContext.Movies.Update(movie);

                movieViewModel.Movies.Insert(index, movie);
            }
            else
            {
                _dbContext.Movies.Add(movie);
                movieViewModel.Movies.Add(movie);
            }

            await _dbContext.SaveChangesAsync();

            Movie.VideoUploadSuccess = true;
            Movie.VideoImageUploadSuccess = true;
            Movie.TrailerUploadSuccess = true;
            Movie.ImageUploadSuccess = true;
            Movie.SearchImageUploadSuccess = true;

            ProcessStarted = false;

            DialogHost.Close("RootDialog");

            await MessageBoxService.Show("Movie saved succesfully!", MessageBoxType.Success);
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

        System.Windows.Application.Current.Dispatcher.Invoke(() => Movie.VideoProgress = 0);
        Movie.VideoImageProgress.Progress = 0;
        Movie.TrailerProgress.Progress = 0;
        Movie.ImageProgress.Progress = 0;
        Movie.SearchImageProgress.Progress = 0;

        if (Movie.VideoUploadSuccess) await _awsStorageManager.DeleteFileAsync(Movie.VideoUrl);
        if (Movie.VideoImageUploadSuccess) await _storageManager.DeleteFileAsync(Movie.VideoImageUrl);
        if (Movie.TrailerUploadSuccess) await _storageManager.DeleteFileAsync(Movie.TrailerUrl);
        if (Movie.ImageUploadSuccess) await _storageManager.DeleteFileAsync(Movie.ImageUrl);
        if (Movie.SearchImageUploadSuccess) await _storageManager.DeleteFileAsync(Movie.SearchImageUrl);
    }
}
