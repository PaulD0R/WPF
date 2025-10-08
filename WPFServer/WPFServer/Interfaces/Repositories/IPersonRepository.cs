using WPFServer.Models;

namespace WPFServer.Interfaces.Repositories
{
    public interface IPersonRepository
    {
        Task<Person?> GetByIdAsync(string id);
        Task<Person?> GetByNameAsync(string name);
        Task<Person?> ChangeUserAsync(string id, Person newPerson);
        Task<ICollection<Person>> GetWithoutRoleUsersAsync(string role);
        Task<Person?> GetLiteByIdAsync(string id);
        Task<ICollection<Person>> GetByNameSimilarAsync(string name);
        Task<ICollection<Person>> GetAllAsync();
        Task<bool> DeleteUserAsync(string id);
        Task<bool> ChangeRoleAsync(string id, string newRole);
        Task<bool?> GetIsLikedByIdAsync(string id, int exerciseId);
        Task<bool> AddRoleByIdAsync(string id, string role);
        Task<bool> DeleteRoleByIdAsync(string id, string role);
        Task<bool> DeleteCommentByIdAsync(string id, int commentId);
    }
}
