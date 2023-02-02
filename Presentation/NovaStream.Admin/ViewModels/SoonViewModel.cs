namespace NovaStream.Admin.ViewModels;

public class SoonViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;

    public ObservableCollection<Soon> Soons { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand<Button> OpenEditDialogHostCommand { get; set; }
    public RelayCommand<Button> DeleteCommand { get; set; }


    public SoonViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        Initialize();
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        Soons = new ObservableCollection<Soon>(_dbContext.Soons);

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand<Button>(button => OpenEditDialogHost(button));
        DeleteCommand = new RelayCommand<Button>(button => Delete(button));
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddSoonViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(Button button)
    {
        // var serial = button?.DataContext as Serial;
        // 
        // ArgumentNullException.ThrowIfNull(serial);

        var model = App.ServiceProvider.GetService<AddSoonViewModel>();

        // model.Serial = serial;
        // model.Season = _dbContext.Seasons.FirstOrDefault(s => s.SerialName == serial.Name);
        // model.Episode = _dbContext.Episodes.FirstOrDefault(e => e.SeasonId == model.Season.Id);
        // 
        await DialogHost.Show(model, "RootDialog");
    }

    private async Task Delete(Button button)
    {
        var soon = button?.DataContext as Soon;

        ArgumentNullException.ThrowIfNull(soon);

        _dbContext.Soons.Remove(soon);
        await _dbContext.SaveChangesAsync();

        Soons.Remove(soon);
    }
}
