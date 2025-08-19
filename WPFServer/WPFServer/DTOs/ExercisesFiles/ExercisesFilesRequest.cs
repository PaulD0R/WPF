using System.ComponentModel.DataAnnotations;

namespace WPFServer.DTOs.ExercisesFiles
{
    public class ExercisesFilesRequest
    {
        [Required]
        public byte[]? TasksFile { get; set; }
    }
}
