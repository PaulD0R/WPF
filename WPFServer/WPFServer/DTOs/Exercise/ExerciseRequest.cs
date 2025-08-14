namespace WPFServer.DTOs.Exercise
{
    public class ExerciseRequest
    {
        public int? Number { get; set; }
        public string? Task { get; set; }
        public byte[]? File {  get; set; }
        public int SubjectId { get; set; }
    }
}
