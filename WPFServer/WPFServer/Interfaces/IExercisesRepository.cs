using WPFServer.DTOs.ExercisesFiles;
using WPFServer.Models;

namespace WPFServer.Interfaces
{
    public interface IExercisesRepository
    {
        Task<ICollection<Exercise>> GetAllAsync();
        Task<Exercise?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<ICollection<Exercise>> GetByPageAsync(int pageNumber);
        ICollection<Exercise> GetByPage(int pageNumber);
        Task<int> GetLengthAsync();
        int GetLength();
        Task AddAsync(Exercise exercise);
        Task<ExercisesFilesDto?> GetTasksFileByIdAsync(int id);
        Task<bool?> ChangeIsLikedAsync(Person person, int id);
        Task<int?> GetLikesCountByIdAsync(int id);
    }
}
