namespace NovaStream.Admin.Views;

public partial class MainView : Window
{
    private bool _isMaximized;


    public MainView()
    {
        InitializeComponent();

        DataContext = App.ServiceProvider.GetService<MainViewModel>();
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
                Width = 1280;
                Height = 780;

                _isMaximized = false;
            }
            else
            {
                WindowState = WindowState.Maximized;

                _isMaximized = true;
            }
        }
    }
}
