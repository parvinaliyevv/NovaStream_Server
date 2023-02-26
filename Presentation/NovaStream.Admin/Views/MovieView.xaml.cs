namespace NovaStream.Admin.Views;

public partial class MovieView : UserControl
{
    public MovieView()
    {
        InitializeComponent();

        DataContext = App.ServiceProvider.GetService<MovieViewModel>();
    }


    private void MovieActorView_ButtonClicked(object sender, RoutedEventArgs e)
    {
        var model = App.ServiceProvider.GetService<MovieActorViewModel>();

        model.Movie = ItemsDataGrid.SelectedItem as Movie;
        model.Initialize();

        App.ServiceProvider.GetService<MainViewModel>().CurrentViewModel = model;
    }

    private void MovieGenreView_ButtonClicked(object sender, RoutedEventArgs e)
    {
        var model = App.ServiceProvider.GetService<MovieGenreViewModel>();

        model.Movie = ItemsDataGrid.SelectedItem as Movie;
        model.Initialize();

        App.ServiceProvider.GetService<MainViewModel>().CurrentViewModel = model;
    }
}
