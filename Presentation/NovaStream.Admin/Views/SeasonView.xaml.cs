namespace NovaStream.Admin.Views;

public partial class SeasonView : UserControl
{
    public SeasonView()
    {
        InitializeComponent();

        DataContext = App.ServiceProvider.GetService<SeasonViewModel>();
    }
}
