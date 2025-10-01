namespace WPFServer.Interfaces.Services;

public interface IStringCachingService
{
    Task SetAsync<T>(string key, T value);
    Task<T?> GetAsync<T>(string key);
    Task<bool> RemoveAsync<T>(string key);
}