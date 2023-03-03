namespace NovaStream.Admin.ViewModels;

public class LoginViewModel : DependencyObject
{
	private readonly IUserManager _userManager;

	public string Email
	{
		get { return (string)GetValue(EmailProperty); }
		set { SetValue(EmailProperty, value); }
	}
	public static readonly DependencyProperty EmailProperty =
		DependencyProperty.Register("Email", typeof(string), typeof(LoginViewModel));

	public string Password { get; set; }

	public RelayCommand SignInCommand { get; set; }


	public LoginViewModel(IUserManager userManager)
	{
		_userManager = userManager;

		SignInCommand = new RelayCommand(() => _ = SignIn());
	}


	private async Task SignIn()
	{
		await Task.CompletedTask;

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password)) { await MessageBoxService.Show("The fields are not filled.", MessageBoxType.Info, "LoginMessageBox"); return; }
		else if (!InternetService.CheckInternet()) { await MessageBoxService.Show("You are not connected to the Internet!", MessageBoxType.Error, "LoginMessageBox"); return; }

        string message = string.Empty;

		_ = MessageBoxService.Show("Sign In...", MessageBoxType.Progress, "LoginMessageBox");

        try
		{            
			var user = await _userManager.FindUserByEmailAsync(Email);

            if (user is null) message = "Incorrect email or password!";
            else if (user.Role != UserRoles.Admin.ToString()) message = "The admin panel can only be accessed as an admin!";

            if (!string.IsNullOrWhiteSpace(message)) { await MessageBoxService.Show(message, MessageBoxType.Error, "LoginMessageBox"); return; }

            var result = await _userManager.CheckPasswordAsync(user, Password);

            if (!result) { await MessageBoxService.Show("Incorrect email or password!", MessageBoxType.Error, "LoginMessageBox"); return; }

			MessageBoxService.Close("LoginMessageBox");

            new MainView(user).Show();
			LoginView.instance.Close();
        }
		catch
		{
			message = !InternetService.CheckInternet() ? "You are not connected to the Internet!" : "Server not responding please try again later!";

            await MessageBoxService.Show(message, MessageBoxType.Error, "LoginMessageBox");
        }
    }
}
