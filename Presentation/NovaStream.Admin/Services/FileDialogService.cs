namespace NovaStream.Admin.Services;

public static class FileDialogService
{
    public static string OpenImageFile()
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "JPG Files(*.jpg)|*.jpg|PNG Files(*.png)|*.png|JPEG Files(*.jpeg)|*.jpeg";
        fileDialog.FilterIndex = 1;

        return (bool)fileDialog.ShowDialog() ? fileDialog.FileName : string.Empty;
    }

    public static string OpenVideoFile()
    {
        var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

        fileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        fileDialog.Filter = "MP4 Files(*.mp4)|*.mp4|AVI Files(*.avi)|*.avi|MOV Files(*.mov)|*.mov";
        fileDialog.FilterIndex = 1;

        return (bool)fileDialog.ShowDialog() ? fileDialog.FileName : string.Empty;
    }
}
