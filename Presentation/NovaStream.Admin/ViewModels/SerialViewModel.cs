﻿namespace NovaStream.Admin.ViewModels;

public class SerialViewModel : ViewModelBase
{
	private readonly AppDbContext _dbContext;

    public ObservableCollection<Serial> Serials { get; set; }

    public RelayCommand OpenAddDialogHostCommand { get; set; }
	public RelayCommand<Button> OpenEditDialogHostCommand { get; set; }
	public RelayCommand<Button> DeleteCommand { get; set; }


	public SerialViewModel(AppDbContext dbContext)
	{
		_dbContext = dbContext;

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
        model.Season = _dbContext.Seasons.FirstOrDefault(s => s.SerialName == serial.Name);
        model.Episode = _dbContext.Episodes.FirstOrDefault(e => e.SeasonId == model.Season.Id);
        
        await DialogHost.Show(model, "RootDialog");
    }

    private async Task Delete(Button button)
    {
		var serial = button?.DataContext as Serial;

		ArgumentNullException.ThrowIfNull(serial);

		_dbContext.Serials.Remove(serial);
		await _dbContext.SaveChangesAsync();

		Serials.Remove(serial);
    }
}
