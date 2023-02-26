namespace NovaStream.Admin.Views;

public partial class ProducerView : UserControl
{
    public ProducerView()
    {
        InitializeComponent();

        DataContext = App.ServiceProvider.GetService<ProducerViewModel>();
    }
}
