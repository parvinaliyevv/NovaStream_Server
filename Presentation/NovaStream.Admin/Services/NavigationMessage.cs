namespace NovaStream.Admin.Services;

public class NavigationMessage 
{
    public Type ViewModelType { get; set; }


    public NavigationMessage(Type viewModelType)
    {
        ViewModelType = viewModelType;
    }
}
