namespace NovaStream.Admin.ViewModels;

public class SoonViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;
    private readonly IStorageManager _storageManager;

    public ObservableCollection<Soon> Soons { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
    public RelayCommand<Button> OpenEditDialogHostCommand { get; set; }
    public RelayCommand<Button> DeleteCommand { get; set; }


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
        var soon = button?.DataContext as Soon;

        ArgumentNullException.ThrowIfNull(soon);

        var model = App.ServiceProvider.GetService<AddSoonViewModel>();

        model.Soon = soon;
        
        await DialogHost.Show(model, "RootDialog");
    }

    private async Task Delete(Button button)
    {
        var soon = button?.DataContext as Soon;

        ArgumentNullException.ThrowIfNull(soon);

        await _storageManager.DeleteFileAsync(soon.TrailerUrl);
        await _storageManager.DeleteFileAsync(soon.TrailerImageUrl);

        _dbContext.Soons.Remove(soon);
        await _dbContext.SaveChangesAsync();

        Soons.Remove(soon);
    }
}
