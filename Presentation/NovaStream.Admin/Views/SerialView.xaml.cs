namespace NovaStream.Admin.Views;

public partial class SerialView : UserControl
{
    public SerialView()
    {
        InitializeComponent();

        DataContext = App.ServiceProvider.GetService<SerialViewModel>();
    }


    private void SerialActorView_ButtonClicked(object sender, RoutedEventArgs e)
    {
        var model = App.ServiceProvider.GetService<SerialActorViewModel>();

        model.Serial = ItemsDataGrid.SelectedItem as Serial;
        model.Initialize();

        App.ServiceProvider.GetService<MainViewModel>().CurrentViewModel = model;
    }

    private void SerialGenreView_ButtonClicked(object sender, RoutedEventArgs e)
    {
        var model = App.ServiceProvider.GetService<SerialGenreViewModel>();

        model.Serial = ItemsDataGrid.SelectedItem as Serial;
        model.Initialize();

        App.ServiceProvider.GetService<MainViewModel>().CurrentViewModel = model;
    }
}
