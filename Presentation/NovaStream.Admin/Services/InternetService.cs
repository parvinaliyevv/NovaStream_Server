namespace NovaStream.Admin.Services;

public static class InternetService
{
    [System.Runtime.InteropServices.DllImport("wininet.dll")]
    private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

    public static bool CheckInternet()
    {
        int desc;
        return InternetGetConnectedState(out desc, 0);
    }
}
