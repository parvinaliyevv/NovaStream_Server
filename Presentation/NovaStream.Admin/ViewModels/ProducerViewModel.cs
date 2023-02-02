namespace NovaStream.Admin.ViewModels;

public class ProducerViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;

    public ObservableCollection<Producer> Producers { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand<Button> OpenEditDialogHostCommand { get; set; }
    public RelayCommand<Button> DeleteCommand { get; set; }


    public ProducerViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        Initialize();
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        Producers = new ObservableCollection<Producer>(_dbContext.Producers);

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand<Button>(button => OpenEditDialogHost(button));
        DeleteCommand = new RelayCommand<Button>(button => Delete(button));
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddProducerViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(Button button)
    {
        // var serial = button?.DataContext as Serial;
        // 
        // ArgumentNullException.ThrowIfNull(serial);

        var model = App.ServiceProvider.GetService<AddProducerViewModel>();

        // model.Serial = serial;
        // model.Season = _dbContext.Seasons.FirstOrDefault(s => s.SerialName == serial.Name);
        // model.Episode = _dbContext.Episodes.FirstOrDefault(e => e.SeasonId == model.Season.Id);
        // 
        await DialogHost.Show(model, "RootDialog");
    }

    private async Task Delete(Button button)
    {
        var producer = button?.DataContext as Producer;

        ArgumentNullException.ThrowIfNull(producer);

        _dbContext.Producers.Remove(producer);
        await _dbContext.SaveChangesAsync();

        Producers.Remove(producer);
    }
}
