namespace NovaStream.Admin.Views;

public partial class MovieView : UserControl
{
    public MovieView()
    {
        InitializeComponent();

        DataContext = App.ServiceProvider.GetService<MovieViewModel>();
    }
}
