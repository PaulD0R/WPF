using WPFServer.DTOs.Exercise;

namespace WPFServer.DTOs.Person
{
    public class FullPersonDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public byte[]? Image { get; set; }
        public ICollection<ExerciseDto>? Exercises { get; set; }
    }
}
