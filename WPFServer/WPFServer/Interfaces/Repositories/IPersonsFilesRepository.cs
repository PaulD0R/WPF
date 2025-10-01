using WPFServer.Models;

namespace WPFServer.Interfaces.Repositories
{
    public interface IPersonsFilesRepository
    {
        public Task<PersonsFiles> CreateNewAsync(string personId);
        public Task<byte[]?> GetPersonsImageByPersonIdAsync(string personId);
        public Task<PersonsFiles?> ChangeImageAsync(string personId, byte[]? image);
        public Task<PersonsFiles?> DeleteImageAsync(string personId);
    }
}
