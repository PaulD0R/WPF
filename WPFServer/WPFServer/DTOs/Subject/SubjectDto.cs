using WPFServer.DTOs.Exercise;

namespace WPFServer.DTOs.Subject
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Year { get; set; }
        public string? Description { get; set; }
        public ICollection<ExerciseDto>? Exercises { get; set; }
    }
}
