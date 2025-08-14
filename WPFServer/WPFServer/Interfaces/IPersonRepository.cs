using WPFServer.Models;

namespace WPFServer.Interfaces
{
    public interface IPersonRepository
    {
        public Task<Person?> GetByIdAsync(string id);
        public Task<Person?> GetByNameAsync(string name);
        public Task<Person?> GetLiteByNameAsync(string name);
        public Task<ICollection<Person>> GetByNameSimilarsAsync(string name);
        public Task<ICollection<Person>> GetAllAsync();
        public Task<ICollection<Comment>?> GetCommentsByNameAsync(string name);
        public Task<bool?> GetIsLikedByIdAsync(string name, int exerciseId);
        public Task<bool> AddRoleByIdAsync(string userId, string role);
        public Task<bool> DeleteRoleByNameAsync(string userName, string role);
        public Task<bool> DeleteCommentByIdAsync(string userName, int commentId);
    }
}
