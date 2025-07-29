namespace WPFServer.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Year { get; set; }
        public string? Description { get; set; }
        public ICollection<Exercise>? Exercises { get; set; }
    }
}
