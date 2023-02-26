namespace NovaStream.Admin.MessageBoxes;

public class ProgressMessageBox : BaseMessageBox
{
	public ProgressMessageBox(string message)
	{
		Title = "Progress";
		Message = message;
	}
}
