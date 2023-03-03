namespace NovaStream.Admin.ViewModels;

public class DirectorViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;

    private int _directorCount;
    public int DirectorCount
    {
        get => _directorCount;
        set { _directorCount = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<Director> _directors;
    public ObservableCollection<Director> Directors
    {
        get => _directors;
        set { _directors = value; RaisePropertyChanged(); }
    }

    public RelayCommand<string> SearchCommand { get; set; }
    public RelayCommand<Director> DeleteCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand<Director> OpenEditDialogHostCommand { get; set; }


    public DirectorViewModel(AppDbContext dbContext, IStorageManager storageManager)
    {
        _dbContext = dbContext;
        _storageManager = storageManager;

        Initialize();

        SearchCommand = new RelayCommand<string>(pattern => Search(pattern));
        DeleteCommand = new RelayCommand<Director>(director => Delete(director));
        RefreshCommand = new RelayCommand(() => Initialize());

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand<Director>(director => OpenEditDialogHost(director));
    }


    private async Task Initialize()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        _ = MessageBoxService.Show($"Loading directors...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            Directors = new ObservableCollection<Director>(_dbContext.Directors);
            DirectorCount = Directors.Count;

            Directors.CollectionChanged += DirectorCountChanged;

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
            var directors = string.IsNullOrWhiteSpace(pattern) ?
            _dbContext.Directors.ToList() :
            _dbContext.Directors.Where(p => (p.Name + " " + p.Surname).Contains(pattern)).ToList();

            if (Directors.Count == directors.Count) return;

            Directors.Clear();

            directors.ForEach(p => Directors.Add(p));
        }
        catch
        {
            await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);
        }
    }

    private async Task Delete(Director director)
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(director);

        _ = MessageBoxService.Show($"Delete <{director.Name} {director.Surname}>...", MessageBoxType.Progress);

        await Task.Delay(1000);

        try
        {
            var imageUrl = director.ImageUrl;

            _dbContext.Directors.Remove(director);
            await _dbContext.SaveChangesAsync();

            await _storageManager.DeleteFileAsync(imageUrl);

            Directors.Remove(director);

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

        var model = App.ServiceProvider.GetService<AddDirectorViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(Director director)
    {
        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        ArgumentNullException.ThrowIfNull(director);

        var model = App.ServiceProvider.GetService<AddDirectorViewModel>();

        model.Director = director.Adapt<DirectorViewModelContent>();

        await DialogHost.Show(model, "RootDialog");
    }

    private void DirectorCountChanged(object? sender, NotifyCollectionChangedEventArgs e) => DirectorCount = Directors.Count;
}
