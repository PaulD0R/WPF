using System.ComponentModel.DataAnnotations;
using WPFServer.DTOs.ExercisesFiles;

namespace WPFServer.DTOs.Exercise
{
    public class NewExerciseRequest
    {
        [Required]
        public int? Number { get; set; }
        [Required]
        public string? Task { get; set; }
        [Required]
        public ExercisesFilesRequest? Files { get; set; }
        [Required]
        public int SubjectId { get; set; }
    }
}
