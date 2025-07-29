using WPFServer.DTOs.ExercisesFiles;
using WPFServer.Models;

namespace WPFServer.DTOs.Exercise
{
    public class NewExerciseRequest
    {
        public int? Number { get; set; }
        public string? Task { get; set; }
        public ExercisesFilesRequest? Files { get; set; }
        public int SubjectId { get; set; }
    }
}
