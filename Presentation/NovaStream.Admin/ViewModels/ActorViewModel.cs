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

    public ObservableCollection<Actor> Actors { get; set; }

    public RelayCommand SearchCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand OpenEditDialogHostCommand { get; set; }


    public ActorViewModel(AppDbContext dbContext, IStorageManager storageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;

        Initialize();
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        Actors = new ObservableCollection<Actor>(_dbContext.Actors);
        ActorCount = Actors.Count;

        Actors.CollectionChanged += ActorCountChanged;

        SearchCommand = new RelayCommand(sender => Search(sender));
        DeleteCommand = new RelayCommand(sender => Delete(sender));

        OpenAddDialogHostCommand = new RelayCommand(_ => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand(sender => OpenEditDialogHost(sender));
    }

    private async Task Search(object sender)
    {
        await Task.CompletedTask;

        var pattern = sender.ToString();

        var actors = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.Actors.ToList() : 
            _dbContext.Actors.Where(a => (a.Name + " " + a.Surname).Contains(pattern)).ToList();

        if (Actors.Count == actors.Count) return;

        Actors.Clear();

        actors.ForEach(a => Actors.Add(a));
    }

    private async Task Delete(object sender)
    {
        var button = sender as Button;
        var actor = button?.DataContext as Actor;

        ArgumentNullException.ThrowIfNull(actor); 
        
        _ = MessageBoxService.Show($"Delete <{actor.Name} {actor.Surname}>...", MessageBoxType.Progress);
        
        await _storageManager.DeleteFileAsync(actor.ImageUrl);

        _dbContext.Actors.Remove(actor);
        await _dbContext.SaveChangesAsync();

        Actors.Remove(actor);

        MessageBoxService.Close();
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddActorViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(object sender)
    {
        var button = sender as Button;
        var actor = button?.DataContext as Actor;

        ArgumentNullException.ThrowIfNull(actor);

        var model = App.ServiceProvider.GetService<AddActorViewModel>();

        model.Actor = actor.Adapt<UploadActorModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private void ActorCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => ActorCount = Actors.Count;
}
