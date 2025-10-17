using WPFServer.DTOs.Exercise;
using WPFServer.DTOs.ExercisesFiles;
using WPFServer.DTOs.Helpers;

namespace WPFServer.Interfaces.Services;

public interface IExerciseService
{
    Task<ICollection<ExerciseDto>> GetAllAsync(string personId);
    Task<ICollection<ExerciseDto>> GetAllWithFiltersAsync(string personId);
    Task<ICollection<ExerciseDto>> GetByPageAsync(int page, string personId, ExerciseHelper helper);
    Task<ICollection<ExerciseDto>> GetExercisesByPersonIdAsync(string personId);
    Task<ICollection<ExerciseDto>> GetExercisesBySubjectAsync(int subjectId, string personId);
    Task<ExercisesFilesDto> GetTaskAsync(int id);
    Task<FullExerciseDto> GetByIdAsync(int id, string personId);
    Task<ExerciseDto> AddAsync(NewExerciseRequest exerciseRequest);
    Task<int> CountAsync();
    Task<int> LikesCountAsync(int id);
    Task<bool> IsLikedAsync(int id, string personId);
    Task<bool> SwitchIsLikedAsync(int id, string personId);
}