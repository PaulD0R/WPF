using WPFTest.MVVM.Model.Exercise;

namespace WPFTest.MVVM.Model.Person
{
    public class PrivatePerson
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public byte[]? Image { get; set; }
        public ICollection<LightExercise>? Exercises { get; set; }
    }
}
