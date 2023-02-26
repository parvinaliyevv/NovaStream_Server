using System.Windows.Data;

namespace NovaStream.Admin.Converters;

public class AzureImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        var storageManager = App.ServiceProvider.GetService<IStorageManager>();

        return storageManager.GetSignedUrl(value.ToString());
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return value;
    }
}
