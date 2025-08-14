namespace WPFServer.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Text { get; set; }
        public DateTime? Date {  get; set; }
        public string? PersonId { get; set; }
        public Person? Person { get; set; }
        public int? ExerciseId { get; set; }
        public Exercise? Exercise { get; set; }
    }
}
