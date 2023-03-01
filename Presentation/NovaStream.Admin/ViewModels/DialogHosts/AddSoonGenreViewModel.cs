namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddSoonGenreViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;

    public List<Soon> Soons { get; set; }
    public List<Genre> Genres { get; set; }
    public SoonGenreViewModelContent SoonGenre { get; set; }

    public bool ProcessStarted
    {
        get { return (bool)GetValue(ProcessStartedProperty); }
        set { SetValue(ProcessStartedProperty, value); }
    }
    public static readonly DependencyProperty ProcessStartedProperty =
        DependencyProperty.Register("ProcessStarted", typeof(bool), typeof(AddSoonGenreViewModel));

    public RelayCommand SaveCommand { get; set; }


    public AddSoonGenreViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        Genres = _dbContext.Genres.ToList();
        SoonGenre = new SoonGenreViewModelContent();

        SaveCommand = new RelayCommand(() => Save());
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error); return; }

        try
        {
            SoonGenre.Verify();

            if (SoonGenre.HasErrors) return;

            ProcessStarted = true;

            var dbSoonGenre = _dbContext.SoonGenres.Include(sg => sg.Genre)
                .FirstOrDefault(sg => sg.SoonName == SoonGenre.Soon.Name && sg.Genre.Id == SoonGenre.Genre.Id);

            if (dbSoonGenre is not null) return;

            var soonGenre = new SoonGenre()
            {
                Soon = SoonGenre.Soon,
                Genre = SoonGenre.Genre
            };

            _dbContext.SoonGenres.Add(soonGenre);
            _dbContext.SaveChanges();

            App.ServiceProvider.GetService<SoonGenreViewModel>()?.SoonGenres.Add(soonGenre);

            ProcessStarted = false;

            DialogHost.Close("RootDialog");

            await MessageBoxService.Show("Soon Genre saved successfully!", MessageBoxType.Success);
        }
        catch
        {
            if (!InternetService.CheckInternet())
                await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error);
            
            else
                await MessageBoxService.Show("Server not responding please try again later!", MessageBoxType.Error);

            ProcessStarted = false;
        }
    }
}
