namespace NovaStream.Admin.Views;

public partial class MainView : Window
{
    private bool _isMaximized;


    public MainView()
    {
        InitializeComponent();

        var context = App.ServiceProvider.GetService<MainViewModel>();

        context.CurrentViewModel = App.ServiceProvider.GetService<MovieViewModel>();

        DataContext = context;
    }


    private void DragWindow_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }

    private void ResizeWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            if (_isMaximized)
            {
                WindowState = WindowState.Normal;
                Width = 1200;
                Height = 700;

                _isMaximized = false;
            }
            else
            {
                WindowState = WindowState.Maximized;

                _isMaximized = true;
            }
        }
    }

    private void CloseApp_ButtonClicked(object sender, RoutedEventArgs e) => Close();
}
