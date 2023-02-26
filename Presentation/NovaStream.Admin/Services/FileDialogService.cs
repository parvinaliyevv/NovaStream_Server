namespace NovaStream.Admin.Services;

public static class FileDialogService
{
    public static string OpenImageFile(string onCancelValue = "")
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "Image Files(*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
        fileDialog.FilterIndex = 1;

        return (bool)fileDialog.ShowDialog() ? fileDialog.FileName : onCancelValue;
    }

    public static string OpenVideoFile(string onCancelValue = "")
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "Video Files(*.mp4;*.mkv;*.avi)|*.mp4;*.mkv;*.avi";
        fileDialog.FilterIndex = 1;

        return (bool)fileDialog.ShowDialog() ? fileDialog.FileName : onCancelValue;
    }
}
