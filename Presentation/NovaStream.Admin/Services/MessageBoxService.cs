namespace NovaStream.Admin.Services;

public static class MessageBoxService
{
    public static Task Show(string message, MessageBoxType type, string identifier = "MessageBox")
    {
        BaseMessageBox model = null;

        switch (type)
        {
            case MessageBoxType.Success:
                model = new SuccessMessageBox(message);
                break;
            case MessageBoxType.Info:
                model = new InfoMessageBox(message);
                break;
            case MessageBoxType.Error:
                model = new ErrorMessageBox(message);
                break;
            case MessageBoxType.Progress:
                model = new ProgressMessageBox(message);
                break;
        }

        return DialogHost.Show(model, identifier);
    }

    public static void Close(string identifier = "MessageBox") => DialogHost.Close(identifier);
}
