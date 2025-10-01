using System.ComponentModel.DataAnnotations;

namespace WPFServer.Models  
{
    public class Subject
    {
        public int Id { get; init; }
        [MaxLength(40)] public string Name { get; init; } =  null!;
        public int Year { get; init; }
        [MaxLength(200)] public string Description { get; init; } = null!;
        public ICollection<Exercise> Exercises { get; init; } = [];
    }
}
