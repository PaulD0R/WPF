using WPFServer.Interfaces.Services;

namespace WPFServer.Interfaces.Managers;

public interface ICachingManager
{
    IStringCachingService String { get; }
    IHashCachingService Hash { get; }
    IListCachingService List { get; }
}