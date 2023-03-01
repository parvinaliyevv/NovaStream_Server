namespace NovaStream.Admin.ViewModels;

public class ActorViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;

    private int _actorCount;
    public int ActorCount
    {
        get => _actorCount;
        set { _actorCount = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<Actor> _actors;
    public ObservableCollection<Actor> Actors
    {
        get => _actors;
        set { _actors = value; RaisePropertyChanged(); }
    }

    public RelayCommand<string> SearchCommand { get; set; }
    public RelayCommand<Actor> DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand<Actor> OpenEditDialogHostCommand { get; set; }


    public ActorViewModel(AppDbContext dbContext, IStorageManager storageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;

        Initialize();
         
        SearchCommand = new RelayCommand<string>(pattern => Search(pattern));
        DeleteCommand = new RelayCommand<Actor>(actor => Delete(actor));
        RefreshCommand = new RelayCommand(() => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand<Actor>(actor => OpenEditDialogHost(actor));

    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        _ = MessageBoxService.Show($"Loading actors...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            Actors = new ObservableCollection<Actor>(_dbContext.Actors);
            ActorCount = Actors.Count;

            Actors.CollectionChanged += ActorCountChanged;

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
            var actors = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.Actors.ToList() :
            _dbContext.Actors.Where(a => (a.Name + " " + a.Surname).Contains(pattern)).ToList();

            if (Actors.Count == actors.Count) return;

            Actors.Clear();

            actors.ForEach(a => Actors.Add(a));
        }
        catch
        {
            await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);
        }
    }

    private async Task Delete(Actor actor)
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(actor);

        _ = MessageBoxService.Show($"Delete <{actor.Name} {actor.Surname}>...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            var imageUrl = actor.ImageUrl;

            _dbContext.Actors.Remove(actor);
            await _dbContext.SaveChangesAsync();

            Actors.Remove(actor);

            await _storageManager.DeleteFileAsync(imageUrl);

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

        var model = App.ServiceProvider.GetService<AddActorViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(Actor actor)
    {
        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(actor);

        var model = App.ServiceProvider.GetService<AddActorViewModel>();

        model.Actor = actor.Adapt<ActorViewModelContent>();

        await DialogHost.Show(model, "RootDialog");
    }

    private void ActorCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => ActorCount = Actors.Count;
}
