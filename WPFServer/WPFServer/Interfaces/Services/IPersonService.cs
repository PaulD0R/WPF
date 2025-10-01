using WPFServer.DTOs.Person;
using WPFServer.DTOs.PersonsFiles;

namespace WPFServer.Interfaces.Services;

public interface IPersonService
{
    Task<FullPersonDto> GetByIdAsync(string id);  
    Task<FullPersonDto> GetByNameAsync(string name);
    Task<FullPrivatePersonDto> GetMeAsync(string id);
    Task<bool> IsLikedAsync(string id, int exerciseId);
    Task<bool> ChangeImageAsync(string id, NewPersonsImageRequest? newImageRequest = null);
}