namespace NovaStream.Admin.ViewModels.Tabs;

public class SerialActorViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;

    private int _serialActorCount;
    public int SerialActorCount
    {
        get => _serialActorCount;
        set { _serialActorCount = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<SerialActor> _serialActors;
    public ObservableCollection<SerialActor> SerialActors
    {
        get => _serialActors;
        set { _serialActors = value; RaisePropertyChanged(); }
    }
    
    public Serial Serial { get; set; }

    public RelayCommand SearchCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }


    public SerialActorViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task Initialize()
    {
        await Task.CompletedTask;

        SerialActors = new ObservableCollection<SerialActor>(_dbContext.SerialActors.Include(sa => sa.Actor).Where(sa => sa.SerialName == Serial.Name));
        SerialActorCount = SerialActors.Count;

        SerialActors.CollectionChanged += SerialActorCountChanged;

        SearchCommand = new RelayCommand(sender => Search(sender));
        DeleteCommand = new RelayCommand(sender => Delete(sender));
        RefreshCommand = new RelayCommand(_ => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(_ => OpenAddDialogHost());
    }

    private async Task Search(object sender)
    {
        await Task.CompletedTask;

        var pattern = sender.ToString();

        var serialActors = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.SerialActors.Include(sa => sa.Actor).Where(sa => sa.SerialName == Serial.Name).ToList() :
            _dbContext.SerialActors.Include(sa => sa.Actor).Where(sa => sa.SerialName == Serial.Name && sa.Actor.Name.Contains(pattern)).ToList();

        if (SerialActors.Count == serialActors.Count) return;

        SerialActors.Clear();

        serialActors.ForEach(sa => SerialActors.Add(sa));
    }

    private async Task Delete(object sender)
    {
        var button = sender as Button;
        var serialActor = button?.DataContext as SerialActor;

        ArgumentNullException.ThrowIfNull(serialActor);

        _ = MessageBoxService.Show($"Delete <{serialActor.Actor.Name} {serialActor.Actor.Surname} from {serialActor.SerialName}>...", MessageBoxType.Progress);

        _dbContext.SerialActors.Remove(serialActor);
        await _dbContext.SaveChangesAsync();

        SerialActors.Remove(serialActor);

        MessageBoxService.Close();
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddSerialActorViewModel>();

        model.SerialActor.Serial = Serial;
        model.Serials = new List<Serial> { Serial };

        var existsActors = await _dbContext.SerialActors.Include(sa => sa.Actor).Where(sa => sa.SerialName == Serial.Name).Select(sa => sa.Actor).ToListAsync();

        if (model.Actors.Count == existsActors.Count)
        {
            await MessageBoxService.Show("Added all possible actors", MessageBoxType.Info);
            return;
        }

        foreach (var actor in existsActors) model.Actors.Remove(actor);

        await DialogHost.Show(model, "RootDialog");
    }

    private void SerialActorCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => SerialActorCount = SerialActors.Count;
}
