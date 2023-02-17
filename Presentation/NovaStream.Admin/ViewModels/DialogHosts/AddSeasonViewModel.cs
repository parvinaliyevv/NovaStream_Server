namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddSeasonViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;

    public UploadSeasonModel Season { get; set; }
    public List<Serial> Serials { get; set; }

    public RelayCommand SaveCommand { get; set; }
    public RelayCommand SelectedSerialChangedCommand { get; set; }


    public AddSeasonViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        Serials = dbContext.Serials.ToList();

        Season = new();

        SaveCommand = new RelayCommand(_ => Save());
        SelectedSerialChangedCommand = new RelayCommand(_ => SelectedSerialChanged());
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        Season.Verify();

        if (Season.HasErrors) return;

        var season = Season.Adapt<Season>();

        _dbContext.Seasons.Add(season);
        await _dbContext.SaveChangesAsync();

        App.ServiceProvider.GetService<SeasonViewModel>().Seasons.Add(season);
    }

    private void SelectedSerialChanged()
    {
        var lastSeasonNumber = _dbContext.Seasons.Max(s => s.Number);

        Season.Number = ++lastSeasonNumber;
    }
}
