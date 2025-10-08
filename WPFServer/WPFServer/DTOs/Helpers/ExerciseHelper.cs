using System.ComponentModel.DataAnnotations;

namespace WPFServer.DTOs.Helpers;

public class ExerciseHelper
{
    [Required] public string SortedBy { get; set; } = null!;
    public ICollection<int>? Numbers { get; set; }
    public ICollection<int>? SubjectsId { get; set; }
}