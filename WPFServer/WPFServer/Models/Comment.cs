using System.ComponentModel.DataAnnotations;

namespace WPFServer.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [MaxLength(15)] public string UserName { get; set; } = null!;
        [MaxLength(200)] public string Text { get; set; } =  null!;
        public DateTime Date {  get; set; } = DateTime.UtcNow;
        [MaxLength(38)] public string PersonId { get; set; } =  null!;
        public Person Person { get; set; } = null!;
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; } =  null!;
    }
}
