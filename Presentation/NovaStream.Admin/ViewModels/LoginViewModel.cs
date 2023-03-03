namespace NovaStream.Admin.ViewModels;

public class LoginViewModel : ViewModelBase
{
	private readonly IUserManager _userManager;

	public LoginViewModelContent Content { get; set; }

	public RelayCommand SignInCommand { get; set; }


	public LoginViewModel(IUserManager userManager)
	{
		_userManager = userManager;

		Content = new();

		SignInCommand = new RelayCommand(() => _ = SignIn());
	}


	private async Task SignIn()
	{
		Content.Verify();

		if (Content.HasErrors) return;

		string message = string.Empty;

        try
		{
            var user = await _userManager.FindUserByEmailAsync(Content.Email);

            if (user is null) message = "Incorrect email or password!";
            else if (user.Role != UserRoles.Admin.ToString()) message = "The control panel can only be accessed as an admin!";

            if (!string.IsNullOrWhiteSpace(message)) { await MessageBoxService.Show(message, MessageBoxType.Error, "LoginMessageBox"); return; }

            var result = await _userManager.CheckPasswordAsync(user, Content.Password);

            if (!result) { await MessageBoxService.Show("Incorrect email or password!", MessageBoxType.Error, "LoginMessageBox"); return; }

            new MainView(user).ShowDialog();
			LoginView.instance.Close();
        }
		catch
		{
			message = !InternetService.CheckInternet() ? "You are not connected to the Internet!" : "Server not responding please try again later!";

            await MessageBoxService.Show(message, MessageBoxType.Error, "LoginMessageBox");
        }
    }
}
