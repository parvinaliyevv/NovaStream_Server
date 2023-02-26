namespace NovaStream.Admin.Views.Tabs;

public partial class SoonGenreView : UserControl
{
    public SoonGenreView()
    {
        InitializeComponent();

        DataContext = App.ServiceProvider.GetService<SoonGenreViewModel>();
    }


    private void SoonView_ButtonClicked(object sender, RoutedEventArgs e)
    {
        App.ServiceProvider.GetService<MainViewModel>().SoonViewCommand.Execute(null);
    }
}
