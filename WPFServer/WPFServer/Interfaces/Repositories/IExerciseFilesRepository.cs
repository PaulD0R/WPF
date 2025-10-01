using WPFServer.Models;

namespace WPFServer.Interfaces.Repositories;

public interface IExerciseFilesRepository
{
    Task<ExercisesFiles?> GetTasksFileByIdAsync(int id);
}