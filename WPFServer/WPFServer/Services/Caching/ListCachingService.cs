using System.Text.Json;
using StackExchange.Redis;
using WPFServer.Interfaces.Services;

namespace WPFServer.Services.Caching;

public class ListCachingService(IDatabase dataBase) : IListCachingService
{
    public async Task PushAsync<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value);
        await dataBase.ListRightPushAsync(key, json);
    }

    public async Task<T?> PopRightAsync<T>(string key)
    {
        var value = await dataBase.ListRightPopAsync(key);
        return value.HasValue ? JsonSerializer.Deserialize<T>(value!) : default;
    }

    public async Task<List<T?>> GetAllAsync<T>(string key)
    {
        var values = await dataBase.ListRangeAsync(key);
        return values.Select(x => x.HasValue ? JsonSerializer.Deserialize<T>(x!) : default).ToList();
    }

    public async Task<bool> ClearAsync(string key)
    {
        return await dataBase.KeyDeleteAsync(key);
    }
}