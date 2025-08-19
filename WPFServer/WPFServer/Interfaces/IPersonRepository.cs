using WPFServer.Models;

namespace WPFServer.Interfaces
{
    public interface IPersonRepository
    {
        public Task<Person?> GetByIdAsync(string id);
        public Task<Person?> GetByNameAsync(string name);
        public Task<Person?> GetLiteByIdAsync(string id);
        public Task<ICollection<Person>> GetByNameSimilarsAsync(string name);
        public Task<ICollection<Person>> GetAllAsync();
        public Task<ICollection<Comment>?> GetCommentsByIdAsync(string id);
        public Task<bool?> GetIsLikedByIdAsync(string id, int exerciseId);
        public Task<bool> AddRoleByIdAsync(string id, string role);
        public Task<bool> DeleteRoleByIdAsync(string id, string role);
        public Task<bool> DeleteCommentByIdAsync(string id, int commentId);
    }
}
