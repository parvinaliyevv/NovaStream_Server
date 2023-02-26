﻿namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddSeasonViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;

    public UploadSeasonModel Season { get; set; }
    public List<Serial> Serials { get; set; }

    public bool ProcessStarted
    {
        get { return (bool)GetValue(ProcessStartedProperty); }
        set { SetValue(ProcessStartedProperty, value); }
    }
    public static readonly DependencyProperty ProcessStartedProperty =
        DependencyProperty.Register("ProcessStarted", typeof(bool), typeof(AddSeasonViewModel));

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

        try
        {
            Season.Verify();

            if (Season.HasErrors) return;

            ProcessStarted = true;

            var season = Season.Adapt<Season>();

            _dbContext.Seasons.Add(season);
            await _dbContext.SaveChangesAsync();

            App.ServiceProvider.GetService<SeasonViewModel>()?.Seasons.Add(season);

            ProcessStarted = false;

            DialogHost.Close("RootDialog");

            await MessageBoxService.Show("Season saved succesfully!", MessageBoxType.Success);
        }
        catch (Exception ex)
        {
            await MessageBoxService.Show(ex.Message, MessageBoxType.Error);

            ProcessStarted = false;
        }
    }

    private void SelectedSerialChanged()
    {
        var lastSeasonNumber = _dbContext.Seasons.Where(s => s.SerialName == Season.Serial.Name).Max(s => s.Number);

        Season.Number = ++lastSeasonNumber;
    }
}
