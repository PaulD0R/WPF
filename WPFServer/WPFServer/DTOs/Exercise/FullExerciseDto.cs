using WPFServer.DTOs.Subject;

namespace WPFServer.DTOs.Exercise
{
    public class FullExerciseDto
    {
        public int Id { get; set; }
        public int? Number { get; set; }
        public string? Task { get; set; }
        public int SubjectId { get; set; }
        public LightSubjectDto? Subject { get; set; }
        public bool IsLiked { get; set; }
    }
}
