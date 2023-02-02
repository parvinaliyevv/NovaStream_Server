namespace NovaStream.Admin.ViewModels;

public class MovieViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;

    public ObservableCollection<Movie> Movies { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand<Button> OpenEditDialogHostCommand { get; set; }
    public RelayCommand<Button> DeleteCommand { get; set; }


    public MovieViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

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
        // var serial = button?.DataContext as Serial;
        // 
        // ArgumentNullException.ThrowIfNull(serial);

        var model = App.ServiceProvider.GetService<AddMovieViewModel>();

        // model.Serial = serial;
        // model.Season = _dbContext.Seasons.FirstOrDefault(s => s.SerialName == serial.Name);
        // model.Episode = _dbContext.Episodes.FirstOrDefault(e => e.SeasonId == model.Season.Id);
        // 
        await DialogHost.Show(model, "RootDialog");
    }

    private async Task Delete(Button button)
    {
        var movie = button?.DataContext as Movie;

        ArgumentNullException.ThrowIfNull(movie);

        _dbContext.Movies.Remove(movie);
        await _dbContext.SaveChangesAsync();

        Movies.Remove(movie);
    }
}
