namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddSerialGenreViewModel
{
    private readonly AppDbContext _dbContext;

    public List<Serial> Serials { get; set; }
    public List<Genre> Genres { get; set; }
    public UploadSerialGenreViewModel SerialGenre { get; set; }

    public RelayCommand SaveCommand { get; set; }


    public AddSerialGenreViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        Genres = _dbContext.Genres.ToList();
        SerialGenre = new UploadSerialGenreViewModel();

        SaveCommand = new RelayCommand(_ => Save());
    }


    private void Save()
    {
        SerialGenre.Verify();

        if (SerialGenre.HasErrors) return;

        var dbSerialGenre = _dbContext.SerialGenres.Include(sg => sg.Genre)
            .FirstOrDefault(sg => sg.SerialName == SerialGenre.Serial.Name && sg.Genre.Id == SerialGenre.Genre.Id);

        if (dbSerialGenre is not null) return;

        var serialGenre = new SerialGenre()
        {
            Serial = SerialGenre.Serial,
            Genre = SerialGenre.Genre
        };

        _dbContext.SerialGenres.Add(serialGenre);
        _dbContext.SaveChanges();

        App.ServiceProvider.GetService<SerialGenreViewModel>().SerialGenres.Add(serialGenre);
    }
}
