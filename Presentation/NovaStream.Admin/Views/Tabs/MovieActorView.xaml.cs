namespace NovaStream.Admin.Views.Tabs;

public partial class MovieActorView : UserControl
{
    public MovieActorView()
    {
        InitializeComponent();

        DataContext = App.ServiceProvider.GetService<MovieActorViewModel>();
    }
    

    private void MovieView_ButtonClicked(object sender, RoutedEventArgs e)
    {
        App.ServiceProvider.GetService<MainViewModel>().MovieViewCommand.Execute(null);
    }

    private void MovieGenreView_ButtonClicked(object sender, RoutedEventArgs e)
    {
        var model = App.ServiceProvider.GetService<MovieGenreViewModel>();

        model.Movie = (DataContext as MovieActorViewModel).Movie;
        model.Initialize();

        App.ServiceProvider.GetService<MainViewModel>().CurrentViewModel = model;
    }
}
