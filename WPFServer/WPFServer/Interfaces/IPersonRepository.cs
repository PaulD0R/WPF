using WPFServer.Models;

namespace WPFServer.Interfaces
{
    public interface IPersonRepository
    {
        public Task<Person?> GetByIdAsync(string id);
        public Task<Person?> GetByNameAsync(string name);
        public Task<ICollection<Person>> GetAllAsync();
        public Task<bool?> GetIsLikedById(string userId, int exerciseId);
    }
}
