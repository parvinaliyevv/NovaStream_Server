namespace NovaStream.Admin.Views;

public partial class ActorView : UserControl
{
    public ActorView()
    {
        InitializeComponent();

        DataContext = App.ServiceProvider.GetService<ActorViewModel>();
    }
}
