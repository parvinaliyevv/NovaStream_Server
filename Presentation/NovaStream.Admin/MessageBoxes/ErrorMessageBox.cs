namespace NovaStream.Admin.MessageBoxes;

public class ErrorMessageBox : BaseMessageBox
{
	public ErrorMessageBox(string message)
	{
		Title = "Error";
		Message = message;
	}
}
