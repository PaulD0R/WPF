using WPFServer.Interfaces.Managers;
using WPFServer.Interfaces.Services;

namespace WPFServer.Managers;

public class CachingManager(
    IStringCachingService @string, IHashCachingService hash, IListCachingService list)
    : ICachingManager
{
    public IStringCachingService String { get; } = @string;
    public IHashCachingService Hash { get; } = hash;
    public IListCachingService List { get; } = list;
}