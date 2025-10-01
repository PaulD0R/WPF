using System.Text.Json;
using StackExchange.Redis;
using WPFServer.Data;
using WPFServer.Interfaces.Services;

namespace WPFServer.Services.Caching;

public class StringCachingService(IDatabase dataBase) : IStringCachingService
{
    public async Task SetAsync<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value);
        await dataBase.StringSetAsync(key, json, TimeSpan.FromMinutes(StaticData.CACHE_TIME));
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await dataBase.StringGetAsync(key);
        return value.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(value.ToString());
    }

    public async Task<bool> RemoveAsync<T>(string key)
    {
        return await dataBase.KeyDeleteAsync(key);
    }
}