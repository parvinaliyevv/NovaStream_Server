namespace NovaStream.Admin.Views.Tabs;

public partial class MovieGenreView : UserControl
{
    public MovieGenreView()
    {
        InitializeComponent();

        DataContext = App.ServiceProvider.GetService<MovieGenreViewModel>();
    }


    private void MovieView_ButtonClicked(object sender, RoutedEventArgs e)
    {
        App.ServiceProvider.GetService<MainViewModel>().MovieViewCommand.Execute(null);
    }

    private void MovieActorView_ButtonClicked(object sender, RoutedEventArgs e)
    {
        var model = App.ServiceProvider.GetService<MovieActorViewModel>();

        model.Movie = (DataContext as MovieGenreViewModel).Movie;
        model.Initialize();

        App.ServiceProvider.GetService<MainViewModel>().CurrentViewModel = model;
    }
}
