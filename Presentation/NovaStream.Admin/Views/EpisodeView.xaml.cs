namespace NovaStream.Admin.Views;

public partial class EpisodeView : UserControl
{
    public EpisodeView()
    {
        InitializeComponent();

        DataContext = App.ServiceProvider.GetService<EpisodeViewModel>();
    }
}
