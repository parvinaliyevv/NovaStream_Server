namespace NovaStream.Admin.Views;

public partial class SerialGenreView : UserControl
{
    public SerialGenreView()
    {
        InitializeComponent();

        DataContext = App.ServiceProvider.GetService<SerialGenreViewModel>();
    }


    private void SerialView_ButtonClicked(object sender, RoutedEventArgs e)
    {
        App.ServiceProvider.GetService<MainViewModel>().SerialViewCommand.Execute(null);
    }

    private void SerialActorView_ButtonClicked(object sender, RoutedEventArgs e)
    {
        var model = App.ServiceProvider.GetService<SerialActorViewModel>();

        model.Serial = (DataContext as SerialGenreViewModel).Serial;
        model.Initialize();

        App.ServiceProvider.GetService<MainViewModel>().CurrentViewModel = model;
    }
}
