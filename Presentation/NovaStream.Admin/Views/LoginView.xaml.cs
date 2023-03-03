namespace NovaStream.Admin.Views;

public partial class LoginView : Window
{
    public static LoginView instance;


    public LoginView()
    {
        InitializeComponent();

        instance = this;

        DataContext = App.ServiceProvider.GetService<LoginViewModel>();
    }


    private void DragWindow_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }

    private void WindowClose_MouseUp(object sender, MouseButtonEventArgs e)
    {
        Close();
    }

    private void ChangePassword_PasswordChanged(object sender, RoutedEventArgs e)
    {
        var context = DataContext as LoginViewModel;
        var passwordBox = sender as PasswordBox;

        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(passwordBox);

        context.Password = passwordBox.Password;
    }
}
