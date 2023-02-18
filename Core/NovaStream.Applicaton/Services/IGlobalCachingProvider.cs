namespace NovaStream.Application.Services;

public interface IGlobalCachingProvider
{
    void AddItem(string key, object value);
    Task AddItemAsync(string key, object value);

    object GetItem(string key);
    Task<object> GetItemAsync(string key);

    object GetItem(string key, bool remove);
    Task<object> GetItemAsync(string key, bool remove);
}
