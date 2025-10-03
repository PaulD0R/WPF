using WPFServer.Models;

namespace WPFServer.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<ICollection<Comment>> GetCommentsByExerciseIdAsync(int exerciseId);
        Task<Comment?> AddAsync(Comment comment);
        Task<bool> DeletePersonCommentByIdAsync(int id, string personId);
        Task<bool> DeleteCommentByIdAsync(int id);
        Task<ICollection<Comment>> GetCommentsByPersonIdAsync(string personId);
        Task<ICollection<Comment>> GetPersonCommentsByExerciseId(int exerciseId, string personId);
    }
}
