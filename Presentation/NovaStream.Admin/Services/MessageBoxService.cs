namespace NovaStream.Admin.Services;

public static class MessageBoxService
{
    public static Task Show(string message, MessageBoxType type, string identifier = "MessageBox")
    {
        if (DialogHost.IsDialogOpen(identifier)) Close();

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

    public static void Close(string identifier = "MessageBox")
    {
        if (DialogHost.IsDialogOpen(identifier))
        {
            DialogHost.Close(identifier);
        }
    }
}
