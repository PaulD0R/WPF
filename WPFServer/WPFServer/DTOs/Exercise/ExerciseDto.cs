namespace WPFServer.DTOs.Exercise
{
    public class ExerciseDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Task { get; set; } = null!;
        public int SubjectId { get; set; }
        public string Subject { get; set; } = null!;
        public bool? IsLiked {  get; set; }
    }
}
