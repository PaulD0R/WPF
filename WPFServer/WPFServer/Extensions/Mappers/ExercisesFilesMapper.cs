using WPFServer.DTOs.ExercisesFiles;
using WPFServer.Models;

namespace WPFServer.Extensions.Mappers
{
    public static class ExercisesFilesMapper
    {
        public static ExercisesFiles ToExercisesFiles(this ExercisesFilesRequest request)
        {
            return new ExercisesFiles
            {
                TasksFile = request.TasksFile ?? []
            };
        }

        public static ExercisesFilesDto ToExercisesFilesDto(this ExercisesFiles request) {
            return new ExercisesFilesDto
            {
                TasksFile = request.TasksFile 
            };
        }
    }
}
