namespace NovaStream.Admin.Views;

public partial class SerialActorView : UserControl
{
    public SerialActorView()
    {
        InitializeComponent();

        DataContext = App.ServiceProvider.GetService<SerialActorViewModel>();
    }


    private void SerialView_ButtonClicked(object sender, RoutedEventArgs e)
    {
        App.ServiceProvider.GetService<MainViewModel>().SerialViewCommand.Execute(null);
    }

    private void SerialGenreView_ButtonClicked(object sender, RoutedEventArgs e)
    {
        var model = App.ServiceProvider.GetService<SerialGenreViewModel>();

        model.Serial = (DataContext as SerialActorViewModel).Serial;
        model.Initialize();

        App.ServiceProvider.GetService<MainViewModel>().CurrentViewModel = model;
    }
}
