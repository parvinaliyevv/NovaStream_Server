namespace NovaStream.Infrastructure.Services;

public class GlobalCachingProvider: CachingProviderBase, IGlobalCachingProvider
{
    #region Singelton (inheriting enabled)

    protected GlobalCachingProvider()
    {

    }

    public static GlobalCachingProvider Instance
    {
        get
        {
            return Nested.instance;
        }
    }

    class Nested
    {
        static Nested()
        {
        }

        internal static readonly GlobalCachingProvider instance = new GlobalCachingProvider();
    }

    #endregion

    #region ICachingProvider

    public virtual new void AddItem(string key, object value)
    {
        base.AddItem(key, value);
    }
    public Task AddItemAsync(string key, object value)
        => Task.Factory.StartNew(() => AddItem(key, value));


    public virtual new void RemoveItem(string key)
    {
        base.RemoveItem(key);
    }
    public Task RemoveItemAsync(string key)
        => Task.Factory.StartNew(() => RemoveItem(key));


    public virtual object GetItem(string key)
    {
        return base.GetItem(key, true);
    }
    public Task<object> GetItemAsync(string key)
        => Task.Factory.StartNew(() => GetItem(key));


    public virtual new object GetItem(string key, bool remove)
    {
        return base.GetItem(key, remove);
    }
    public Task<object> GetItemAsync(string key, bool remove)
        => Task.Factory.StartNew(() => GetItem(key, remove));

    #endregion
}
