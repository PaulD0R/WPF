using Microsoft.EntityFrameworkCore;
using WPFServer.Context;
using WPFServer.Interfaces.Repositories;
using WPFServer.Models;

namespace WPFServer.Repositories;

public class ExerciseFilesRepository(ApplicationContext context) : IExerciseFilesRepository
{
    public async Task<ExercisesFiles?> GetTasksFileByIdAsync(int id)
    {
        return await context.ExercisesFiles.FirstOrDefaultAsync(x => x.ExerciseId == id);
    }
}  