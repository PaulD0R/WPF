using WPFServer.DTOs.Subject;

namespace WPFServer.DTOs.Exercise
{
    public class FullExerciseDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Task { get; set; } = null!;
        public int SubjectId { get; set; }
        public LiteSubjectDto Subject { get; set; } = null!;
        public bool IsLiked { get; set; }
    }
}
