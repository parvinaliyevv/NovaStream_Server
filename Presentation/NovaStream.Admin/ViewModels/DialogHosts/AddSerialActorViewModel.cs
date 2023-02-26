﻿namespace NovaStream.Admin.ViewModels.DialogHosts;

public class AddSerialActorViewModel : DependencyObject
{
    private readonly AppDbContext _dbContext;

    public List<Serial> Serials { get; set; }
    public List<Actor> Actors { get; set; }
    public UploadSerialActorViewModel SerialActor { get; set; }

    public bool ProcessStarted
    {
        get { return (bool)GetValue(ProcessStartedProperty); }
        set { SetValue(ProcessStartedProperty, value); }
    }
    public static readonly DependencyProperty ProcessStartedProperty =
        DependencyProperty.Register("ProcessStarted", typeof(bool), typeof(AddSerialActorViewModel));

    public RelayCommand SaveCommand { get; set; }


    public AddSerialActorViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        Actors = _dbContext.Actors.ToList();
        SerialActor = new UploadSerialActorViewModel();

        SaveCommand = new RelayCommand(_ => Save());
    }


    private async Task Save()
    {
        await Task.CompletedTask;

        try
        {
            SerialActor.Verify();

            if (SerialActor.HasErrors) return;

            ProcessStarted = true;

            var dbSerialActor = _dbContext.SerialActors.Include(sa => sa.Actor)
                .FirstOrDefault(sa => sa.SerialName == SerialActor.Serial.Name && sa.Actor.Id == SerialActor.Actor.Id);

            if (dbSerialActor is not null) return;

            var serialActor = new SerialActor()
            {
                Serial = SerialActor.Serial,
                Actor = SerialActor.Actor
            };

            _dbContext.SerialActors.Add(serialActor);
            _dbContext.SaveChanges();

            App.ServiceProvider.GetService<SerialActorViewModel>()?.SerialActors.Add(serialActor);

            ProcessStarted = false;

            DialogHost.Close("RootDialog");

            await MessageBoxService.Show("Serial Actor saved successfully!", MessageBoxType.Success);
        }
        catch (Exception ex)
        {
            await MessageBoxService.Show(ex.Message, MessageBoxType.Error);

            ProcessStarted = false;
        }
    }
}