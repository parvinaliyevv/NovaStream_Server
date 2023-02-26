namespace NovaStream.Admin.Views;

public partial class SoonView : UserControl
{
    public SoonView()
    {
        InitializeComponent();

        DataContext = App.ServiceProvider.GetService<SoonViewModel>();
    }


    private void SoonGenreView_ButtonClicked(object sender, RoutedEventArgs e)
    {
        var model = App.ServiceProvider.GetService<SoonGenreViewModel>();

        model.Soon = ItemsDataGrid.SelectedItem as Soon;
        model.Initialize();

        App.ServiceProvider.GetService<MainViewModel>().CurrentViewModel = model;
    }
}
