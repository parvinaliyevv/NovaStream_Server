namespace NovaStream.Admin.ViewModels;

public class MovieViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;
    private readonly IAWSStorageManager _awsStorageManager;

    public ObservableCollection<Movie> Movies { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand<Button> OpenEditDialogHostCommand { get; set; }
    public RelayCommand<Button> DeleteCommand { get; set; }


    public MovieViewModel(AppDbContext dbContext, IStorageManager storageManager, IAWSStorageManager awsStorageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;
        _awsStorageManager = awsStorageManager;

        Initialize();
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        Movies = new ObservableCollection<Movie>(_dbContext.Movies.Include(s => s.Producer));

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand<Button>(button => OpenEditDialogHost(button));
        DeleteCommand = new RelayCommand<Button>(button => Delete(button));
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddMovieViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(Button button)
    {
        var movie = button?.DataContext as Movie;

        ArgumentNullException.ThrowIfNull(movie);

        var model = App.ServiceProvider.GetService<AddMovieViewModel>();

        model.Movie = movie;

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task Delete(Button button)
    {
        var movie = button?.DataContext as Movie;

        ArgumentNullException.ThrowIfNull(movie);

        await _awsStorageManager.DeleteFileAsync(movie.VideoUrl);
        await _storageManager.DeleteFileAsync(movie.VideoImageUrl);
        await _storageManager.DeleteFileAsync(movie.TrailerUrl);
        await _storageManager.DeleteFileAsync(movie.ImageUrl);
        await _storageManager.DeleteFileAsync(movie.SearchImageUrl);

        _dbContext.Movies.Remove(movie);
        await _dbContext.SaveChangesAsync();

        Movies.Remove(movie);
    }
}
