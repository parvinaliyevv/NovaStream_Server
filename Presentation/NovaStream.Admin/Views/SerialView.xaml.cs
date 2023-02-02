namespace NovaStream.Admin.Views;

public partial class SerialView : UserControl
{
    public SerialView()
    {
        InitializeComponent();

        DataContext = App.ServiceProvider.GetService<SerialViewModel>();
    }
}
