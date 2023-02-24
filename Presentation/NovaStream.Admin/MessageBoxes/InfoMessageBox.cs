namespace NovaStream.Admin.MessageBoxes;

public class InfoMessageBox : BaseMessageBox
{
	public InfoMessageBox(string message)
	{
		Title = "Info";
		Message = message;
	}
}
