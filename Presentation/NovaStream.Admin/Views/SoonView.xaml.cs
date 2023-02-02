namespace NovaStream.Admin.Views;

public partial class SoonView : UserControl
{
    public SoonView()
    {
        InitializeComponent();

        DataContext = App.ServiceProvider.GetService<SoonViewModel>();
    }
}
