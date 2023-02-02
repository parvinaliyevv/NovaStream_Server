namespace NovaStream.Admin.ViewModels;

public class ActorViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;

    public ObservableCollection<Actor> Actors { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand<Button> OpenEditDialogHostCommand { get; set; }
    public RelayCommand<Button> DeleteCommand { get; set; }


    public ActorViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        Initialize();
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        Actors = new ObservableCollection<Actor>(_dbContext.Actors);

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand<Button>(button => OpenEditDialogHost(button));
        DeleteCommand = new RelayCommand<Button>(button => Delete(button));
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddActorViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(Button button)
    {
        // var serial = button?.DataContext as Serial;
        // 
        // ArgumentNullException.ThrowIfNull(serial);

        var model = App.ServiceProvider.GetService<AddActorViewModel>();

        // model.Serial = serial;
        // model.Season = _dbContext.Seasons.FirstOrDefault(s => s.SerialName == serial.Name);
        // model.Episode = _dbContext.Episodes.FirstOrDefault(e => e.SeasonId == model.Season.Id);
        // 
        await DialogHost.Show(model, "RootDialog");
    }

    private async Task Delete(Button button)
    {
        var actor = button?.DataContext as Actor;

        ArgumentNullException.ThrowIfNull(actor);

        _dbContext.Actors.Remove(actor);
        await _dbContext.SaveChangesAsync();

        Actors.Remove(actor);
    }
}
