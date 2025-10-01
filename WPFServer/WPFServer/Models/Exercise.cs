using System.ComponentModel.DataAnnotations;

namespace WPFServer.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public int Number { get; set; }
        [MaxLength(200)] public string Task { get; set; } = null!;
        public ExercisesFiles ExercisesFiles { get; set; } = null!;
        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;
        public ICollection<Person> Persons { get; set; } = [];
        public ICollection<Comment> Comments { get; set; } = [];
    }
}
