namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddSeasonViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;

    public Season Season { get; set; }
    public List<Serial> Serials { get; set; }

    public int Number
    {
        get { return (int)GetValue(NumberProperty); }
        set { SetValue(NumberProperty, value); }
    }
    public static readonly DependencyProperty NumberProperty =
        DependencyProperty.Register("Number", typeof(int), typeof(AddSeasonViewModel));

    public RelayCommand SaveCommand { get; set; }
    public RelayCommand SelectedSerialChangedCommand { get; set; }


    public AddSeasonViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        Season = new Season();
        Serials = dbContext.Serials.ToList();

        Season.Serial = Serials[0];

        SaveCommand = new RelayCommand(() => Save());
        SelectedSerialChangedCommand = new RelayCommand(() => SelectedSerialChanged());

        SelectedSerialChanged();
    }


    private async Task Save()
    {
        Season? dbSeason = _dbContext.Seasons.FirstOrDefault(s => s.SerialName == Season.SerialName);

        if (dbSeason is null)
            Season.SerialName = Season.Serial.Name;

        if (dbSeason is not null) _dbContext.Seasons.Remove(Season);

        Season.Number = Number;

        _dbContext.Seasons.Add(Season);
        await _dbContext.SaveChangesAsync();
    }

    private void SelectedSerialChanged()
    {
        var lastSeasonNumber = _dbContext.Seasons.Where(s => s.SerialName == Season.Serial.Name)
            .OrderBy(s => s.Number).Last().Number;

        Number = lastSeasonNumber + 1;
    }
}
