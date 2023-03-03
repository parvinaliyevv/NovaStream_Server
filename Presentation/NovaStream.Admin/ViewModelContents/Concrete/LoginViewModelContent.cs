namespace NovaStream.Admin.ViewModelContents.Concrete;

public class LoginViewModelContent : ViewModelContentBase
{
	private string _email;
	public string Email
	{
		get => _email;
		set
		{
            _email = value;

            OnPropertyChanged();

            ClearErrors(nameof(Email));

			
            if (string.IsNullOrWhiteSpace(_email)) AddError(nameof(Email), $"{nameof(Email)} cannot be empty!");

            // var regex = new Regex("^\\w+(\\.-?\\w+)@\\w+([\\.-]?\\w+)(\\.\\w{2,3})+$");
            // var result = regex.Match(_email);
			// 
            // if (!result.Success) AddError(nameof(Email), $"wrong email address!");
        }
    }

	private string _password;
	public string Password
	{
		get => _password;
		set
		{
            _password = value;

            OnPropertyChanged();

            ClearErrors(nameof(Password));

            if (string.IsNullOrWhiteSpace(_password)) AddError(nameof(Password), $"{nameof(Password)} cannot be empty!");
        }
	}


	public override void Verify()
	{
		Email = Email;
		Password = Password;
	}
}
