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

    private ObservableCollection<Soon> _soons;
    public ObservableCollection<Soon> Soons
    {
        get => _soons;
        set { _soons = value; RaisePropertyChanged(); }
    }

    public RelayCommand<string> SearchCommand { get; set; }
    public RelayCommand<Soon> DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand<Soon> OpenEditDialogHostCommand { get; set; }


    public SoonViewModel(AppDbContext dbContext, IStorageManager storageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;

        Initialize();

        SearchCommand = new RelayCommand<string>(pattern => Search(pattern));
        DeleteCommand = new RelayCommand<Soon>(soon => Delete(soon));
        RefreshCommand = new RelayCommand(() => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand<Soon>(soon => OpenEditDialogHost(soon));
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        _ = MessageBoxService.Show($"Loading soons...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            Soons = new ObservableCollection<Soon>(_dbContext.Soons);
            SoonCount = Soons.Count;

            Soons.CollectionChanged += SoonCountChanged;

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
            var soons = string.IsNullOrWhiteSpace(pattern) ?
           _dbContext.Soons.ToList() :
           _dbContext.Soons.Where(s => s.Name.Contains(pattern)).ToList();

            if (Soons.Count == soons.Count) return;

            Soons.Clear();

            soons.ForEach(s => Soons.Add(s));
        }
        catch
        {
            await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);
        }
    }

    private async Task Delete(Soon soon)
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(soon);

        _ = MessageBoxService.Show($"Delete <{soon.Name}>...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            var trailerUrl = soon.TrailerUrl;
            var trailerImageUrl = soon.TrailerImageUrl;

            _dbContext.Soons.Remove(soon);
            await _dbContext.SaveChangesAsync();

            Soons.Remove(soon);

            await _storageManager.DeleteFileAsync(trailerUrl);
            await _storageManager.DeleteFileAsync(trailerImageUrl);

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

        var model = App.ServiceProvider.GetService<AddSoonViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(Soon soon)
    {
        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(soon);

        var model = App.ServiceProvider.GetService<AddSoonViewModel>();

        model.Soon = soon.Adapt<SoonViewModelContent>();
        model.IsEdit = true;

        await DialogHost.Show(model, "RootDialog");
    }

    private void SoonCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => SoonCount = Soons.Count;
}
