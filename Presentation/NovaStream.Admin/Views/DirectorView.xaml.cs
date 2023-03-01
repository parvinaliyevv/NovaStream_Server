namespace NovaStream.Admin.Views;

public partial class DirectorView : UserControl
{
    public DirectorView()
    {
        InitializeComponent();

        DataContext = App.ServiceProvider.GetService<DirectorViewModel>();
    }
}
