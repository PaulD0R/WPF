using WPFServer.Models;

namespace WPFServer.Interfaces.Repositories
{
    public interface IAdminRepository
    {
        Task<ICollection<Person>> GetAllUsersAsync();
        Task<ICollection<Comment>?> GetCommentsByPersonIdAsync(string id);
        Task<Person?> ChangeUserAsync(string id, Person newPerson);
        Task<bool> ChangeRoleAsync(string id, string newRole);
        Task<bool> DeleteUserAsync(string id);
        Task<bool> DeleteCommentAsync(int id);
    }
}
