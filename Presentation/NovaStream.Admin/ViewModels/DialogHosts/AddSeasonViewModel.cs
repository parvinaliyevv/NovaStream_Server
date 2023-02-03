namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddSeasonViewModel : ViewModelBase
{
    private readonly AppDbContext _dbContext;


    public Season Season { get; set; }
    public List<Serial> Serials { get; set; }


    public RelayCommand SaveCommand { get; set; }


    public AddSeasonViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        Season = new Season();
        Serials = dbContext.Serials.ToList();

        SaveCommand = new RelayCommand(() => Save());
    }

    private async Task Save()
    {
        Season? dbSeason = _dbContext.Seasons.FirstOrDefault(s => s.SerialName == Season.SerialName);

        if (dbSeason is null)
            Season.SerialName = Season.Serial.Name;

        if (dbSeason is not null) _dbContext.Seasons.Remove(Season);

        _dbContext.Seasons.Add(Season);
        await _dbContext.SaveChangesAsync();
    }
}
