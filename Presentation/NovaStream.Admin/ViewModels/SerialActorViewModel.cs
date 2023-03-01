namespace NovaStream.Admin.ViewModels;

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

    public RelayCommand<string> SearchCommand { get; set; }
    public RelayCommand<SerialActor> DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }


    public SerialActorViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        SearchCommand = new RelayCommand<string>(pattern => Search(pattern));
        DeleteCommand = new RelayCommand<SerialActor>(serialActor => Delete(serialActor));
        RefreshCommand = new RelayCommand(() => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
    }


    public async Task Initialize()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        _ = MessageBoxService.Show($"Loading serial actors...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            SerialActors = new ObservableCollection<SerialActor>(_dbContext.SerialActors.Include(sa => sa.Actor).Where(sa => sa.SerialName == Serial.Name));
            SerialActorCount = SerialActors.Count;

            SerialActors.CollectionChanged += SerialActorCountChanged;

            MessageBoxService.Close();
        }
        catch
        {
            await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);
        }
    }

    private async Task Search(string pattern)
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        try
        {
            var serialActors = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.SerialActors.Include(sa => sa.Actor).Where(sa => sa.SerialName == Serial.Name).ToList() :
            _dbContext.SerialActors.Include(sa => sa.Actor).Where(sa => sa.SerialName == Serial.Name && sa.Actor.Name.Contains(pattern)).ToList();

            if (SerialActors.Count == serialActors.Count) return;

            SerialActors.Clear();

            serialActors.ForEach(sa => SerialActors.Add(sa));
        }
        catch
        {
            await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);
        }
    }

    private async Task Delete(SerialActor serialActor)
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(serialActor);

        _ = MessageBoxService.Show($"Delete <{serialActor.Actor.Name} {serialActor.Actor.Surname} from {serialActor.SerialName}>...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            _dbContext.SerialActors.Remove(serialActor);
            await _dbContext.SaveChangesAsync();

            SerialActors.Remove(serialActor);

            MessageBoxService.Close();
        }
        catch
        {
            await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);
        }
    }

    private async Task OpenAddDialogHost()
    {
        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

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
