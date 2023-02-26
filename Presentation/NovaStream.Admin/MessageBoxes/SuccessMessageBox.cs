namespace NovaStream.Admin.MessageBoxes;

public class SuccessMessageBox : BaseMessageBox
{
	public SuccessMessageBox(string message)
	{
		Title = "Success";
		Message = message;
	}
}
