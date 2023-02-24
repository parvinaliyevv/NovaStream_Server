namespace NovaStream.Admin.ViewModels;

public class SoonViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;

    private int _soonCount;
    public int SoonCount
    {
        get => _soonCount;
        set { _soonCount = value; RaisePropertyChanged(); }
    }

    public ObservableCollection<Soon> Soons { get; set; }

    public RelayCommand SearchCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand OpenEditDialogHostCommand { get; set; }


    public SoonViewModel(AppDbContext dbContext, IStorageManager storageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;

        Initialize();
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        Soons = new ObservableCollection<Soon>(_dbContext.Soons);
        SoonCount = Soons.Count;

        Soons.CollectionChanged += SoonCountChanged;

        SearchCommand = new RelayCommand(sender => Search(sender));
        DeleteCommand = new RelayCommand(sender => Delete(sender));

        OpenAddDialogHostCommand = new RelayCommand(_ => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand(sender => OpenEditDialogHost(sender));
    }

    private async Task Search(object sender)
    {
        await Task.CompletedTask;

        var pattern = sender.ToString();

        var soons = string.IsNullOrWhiteSpace(pattern) ? 
            _dbContext.Soons.ToList() : 
            _dbContext.Soons.Where(s => s.Name.Contains(pattern)).ToList();

        if (Soons.Count == soons.Count) return;

        Soons.Clear();

        soons.ForEach(s => Soons.Add(s));
    }

    private async Task Delete(object sender)
    {
        var button = sender as Button;
        var soon = button?.DataContext as Soon;

        ArgumentNullException.ThrowIfNull(soon);

        _ = MessageBoxService.Show($"Delete <{soon.Name}>...", MessageBoxType.Progress);

        await _storageManager.DeleteFileAsync(soon.TrailerUrl);
        await _storageManager.DeleteFileAsync(soon.TrailerImageUrl);

        _dbContext.Soons.Remove(soon);
        await _dbContext.SaveChangesAsync();

        Soons.Remove(soon);

        MessageBoxService.Close();
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddSoonViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(object sender)
    {
        var button = sender as Button;
        var soon = button?.DataContext as Soon;

        ArgumentNullException.ThrowIfNull(soon);

        var model = App.ServiceProvider.GetService<AddSoonViewModel>();

        model.Soon = soon.Adapt<UploadSoonModel>();
        model.IsEdit = true;
        
        await DialogHost.Show(model, "RootDialog");
    }

    private void SoonCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => SoonCount = Soons.Count;
}
