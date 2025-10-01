using WPFServer.Interfaces.Services;

namespace WPFServer.Interfaces.Managers;

public interface ICachingService
{
    IStringCachingService String { get; }
    IHashCachingService Hash { get; }
    IListCachingService List { get; }
}