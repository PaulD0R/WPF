namespace WPFServer.Interfaces.Services;

public interface IListCachingService
{
    Task PushAsync<T>(string key, T value);
    Task<T?> PopRightAsync<T>(string key);
    Task<List<T?>> GetAllAsync<T>(string key);
    Task<bool> ClearAsync(string key);
}