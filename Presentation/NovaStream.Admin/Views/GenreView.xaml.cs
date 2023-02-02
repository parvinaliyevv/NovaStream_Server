namespace NovaStream.Admin.Views;

public partial class GenreView : UserControl
{
    public GenreView()
    {
        InitializeComponent();

        DataContext = App.ServiceProvider.GetService<GenreViewModel>();
    }
}
