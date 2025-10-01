using WPFServer.Models;

namespace WPFServer.Interfaces.Repositories
{
    public interface IExerciseRepository
    {
        Task<ICollection<Exercise>> GetAllAsync();
        Task<Exercise?> GetByIdAsync(int id);
        Task<ICollection<Exercise>> GetByPersonIdAsync(string personId);
        Task<ICollection<Exercise>> GetBySubjectIdAsync(int subjectId);
        Task<bool> DeleteAsync(int id);
        Task<ICollection<Exercise>> GetByPageAsync(int pageNumber);
        ICollection<Exercise> GetByPage(int pageNumber);
        Task<int> GetCountAsync();
        int GetLength();
        Task<bool> AddAsync(Exercise exercise);
        Task<bool?> ChangeIsLikedAsync(string personId, int id);
        Task<int?> GetLikesCountByIdAsync(int id);
    }
}
