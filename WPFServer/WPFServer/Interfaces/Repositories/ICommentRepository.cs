using WPFServer.Models;

namespace WPFServer.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        public Task<ICollection<Comment>> GetCommentsByExerciseIdAsync(int exerciseId);
        public Task<Comment?> AddAsync(Comment comment);
        public Task<bool> DeleteByIdAsync(int id, string personId);
        public Task<ICollection<Comment>> GetCommentsByPersonIdAsync(string personId);
        public Task<ICollection<Comment>> GetPersonCommentsByExerciseId(int exerciseId, string personId);
    }
}
