namespace WPFServer.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public int? Number { get; set; }
        public string? Task { get; set; }
        public ExercisesFiles? ExercisesFiles { get; set; }
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
        public ICollection<Person>? Persons { get; set; }
        public ICollection<Comment>? Comments {  get; set; }
    }
}
