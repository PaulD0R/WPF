using WPFServer.Models;

namespace WPFServer.Interfaces.Repositories
{
    public interface IPersonsFilesRepository
    {
        Task<PersonsFiles> CreateNewAsync(string personId);
        Task<byte[]?> GetPersonsImageByPersonIdAsync(string personId);
        Task<PersonsFiles?> ChangeImageAsync(string personId, byte[]? image);
        Task<PersonsFiles?> DeleteImageAsync(string personId);
    }
}
