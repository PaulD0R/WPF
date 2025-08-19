using System.ComponentModel.DataAnnotations;

namespace WPFServer.DTOs.Exercise
{
    public class ExerciseRequest
    {
        [Required]
        public int? Number { get; set; }
        [Required]
        public string? Task { get; set; }
        [Required]
        public byte[]? File {  get; set; }
        [Required]
        public int SubjectId { get; set; }
    }
}
