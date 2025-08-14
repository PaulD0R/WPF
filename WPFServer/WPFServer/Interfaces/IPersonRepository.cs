using WPFServer.Models;

namespace WPFServer.Interfaces
{
    public interface IPersonRepository
    {
        public Task<Person?> GetByIdAsync(string id);
        public Task<Person?> GetByNameAsync(string name);
        public Task<ICollection<Person>> GetByNameSimilarsAsync(string name);
        public Task<ICollection<Person>> GetAllAsync();
        public Task<bool?> GetIsLikedByIdAsync(string userId, int exerciseId);
        public Task<bool> AddRoleByIdAsync(string userId, string role);
        public Task<bool> DeleteRoleByNameAsync(string userId, string role);
    }
}
