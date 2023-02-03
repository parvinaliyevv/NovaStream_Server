namespace NovaStream.Admin.ViewModels;

public class GenreViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;

    public ObservableCollection<Genre> Genres { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand<Button> OpenEditDialogHostCommand { get; set; }
    public RelayCommand<Button> DeleteCommand { get; set; }


    public GenreViewModel(AppDbContext dbContext, IStorageManager storageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;

        Initialize();
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        Genres = new ObservableCollection<Genre>(_dbContext.Genres);

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand<Button>(button => OpenEditDialogHost(button));
        DeleteCommand = new RelayCommand<Button>(button => Delete(button));
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddGenreViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(Button button)
    {
        var genre = button?.DataContext as Genre;

        ArgumentNullException.ThrowIfNull(genre);

        var model = App.ServiceProvider.GetService<AddGenreViewModel>();

        model.Genre = genre;

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task Delete(Button button)
    {
        var genre = button?.DataContext as Genre;

        ArgumentNullException.ThrowIfNull(genre);

        await _storageManager.DeleteFileAsync(genre.ImageUrl);

        _dbContext.Genres.Remove(genre);
        await _dbContext.SaveChangesAsync();

        Genres.Remove(genre);
    }
}
