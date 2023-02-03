namespace NovaStream.Admin.ViewModels;

public class SerialViewModel : ViewModelBase
{
	private readonly AppDbContext _dbContext;
	private readonly IStorageManager _storageManager;
	private readonly IAWSStorageManager _awsStorageManager;

    public ObservableCollection<Serial> Serials { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
	public RelayCommand<Button> OpenEditDialogHostCommand { get; set; }
	public RelayCommand<Button> DeleteCommand { get; set; }


	public SerialViewModel(AppDbContext dbContext, IStorageManager storageManager, IAWSStorageManager awsStorageManager)
	{
		_dbContext = dbContext;
        _storageManager = storageManager;
        _awsStorageManager = awsStorageManager;

		Initialize();
	}


	private async Task Initialize()
	{
        await Task.CompletedTask;

		Serials = new ObservableCollection<Serial>(_dbContext.Serials.Include(s => s.Producer));

        OpenAddDialogHostCommand = new RelayCommand(() => OpenAddDialogHost());
        OpenEditDialogHostCommand = new RelayCommand<Button>(button => OpenEditDialogHost(button));
		DeleteCommand = new RelayCommand<Button>(button => Delete(button));
    }

    private async Task OpenAddDialogHost()
    {
        var model = App.ServiceProvider.GetService<AddSerialViewModel>();

        await DialogHost.Show(model, "RootDialog");
    }

    private async Task OpenEditDialogHost(Button button)
    {
        var serial = button?.DataContext as Serial;

        ArgumentNullException.ThrowIfNull(serial);

        var model = App.ServiceProvider.GetService<AddSerialViewModel>();

        model.Serial = serial;

        if (serial is not null)
        {
            model.Season = await _dbContext.Seasons.FirstOrDefaultAsync(s => s.SerialName == serial.Name);

            if (model.Season is not null)  model.Episode = await _dbContext.Episodes.FirstOrDefaultAsync(e => e.SeasonId == model.Season.Id);
        }
        
        await DialogHost.Show(model, "RootDialog");
    }

    private async Task Delete(Button button)
    {
		var serial = button?.DataContext as Serial;

		ArgumentNullException.ThrowIfNull(serial);

        var episodes = _dbContext.Episodes.Include(e => e.Season)
            .Where(e => e.Season.SerialName == serial.Name);

        foreach (var episode in episodes)
        {
            await _storageManager.DeleteFileAsync(episode.ImageUrl);
            await _awsStorageManager.DeleteFileAsync(episode.VideoUrl);
        }

        await _storageManager.DeleteFileAsync(serial.TrailerUrl);
        await _storageManager.DeleteFileAsync(serial.ImageUrl);
        await _storageManager.DeleteFileAsync(serial.SearchImageUrl);

		_dbContext.Serials.Remove(serial);
		await _dbContext.SaveChangesAsync();

		Serials.Remove(serial);
    }
}
