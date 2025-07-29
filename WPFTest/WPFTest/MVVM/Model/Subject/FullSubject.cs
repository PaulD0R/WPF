using WPFTest.MVVM.Model.Exercise;

namespace WPFTest.MVVM.Model.Subject
{
    public class FullSubject
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Year { get; set; }
        public string? Description { get; set; }
        public ICollection<LightExercise>? Exercises { get; set; }
    }
}
