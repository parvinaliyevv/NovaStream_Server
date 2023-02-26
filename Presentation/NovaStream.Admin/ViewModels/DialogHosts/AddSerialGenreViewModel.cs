﻿namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddSerialGenreViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;

    public List<Serial> Serials { get; set; }
    public List<Genre> Genres { get; set; }
    public UploadSerialGenreViewModel SerialGenre { get; set; }

    public bool ProcessStarted
    {
        get { return (bool)GetValue(ProcessStartedProperty); }
        set { SetValue(ProcessStartedProperty, value); }
    }
    public static readonly DependencyProperty ProcessStartedProperty =
        DependencyProperty.Register("ProcessStarted", typeof(bool), typeof(AddSerialGenreViewModel));

    public RelayCommand SaveCommand { get; set; }


    public AddSerialGenreViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        Genres = _dbContext.Genres.ToList();
        SerialGenre = new UploadSerialGenreViewModel();

        SaveCommand = new RelayCommand(_ => Save());
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        try
        {
            SerialGenre.Verify();

            if (SerialGenre.HasErrors) return;

            ProcessStarted = true;

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

            App.ServiceProvider.GetService<SerialGenreViewModel>()?.SerialGenres.Add(serialGenre);

            ProcessStarted = false;

            DialogHost.Close("RootDialog");

            await MessageBoxService.Show("Serial Genre saved successfully!", MessageBoxType.Success);
        }
        catch (Exception ex)
        {
            await MessageBoxService.Show(ex.Message, MessageBoxType.Error);

            ProcessStarted = false;
        }
    }
}