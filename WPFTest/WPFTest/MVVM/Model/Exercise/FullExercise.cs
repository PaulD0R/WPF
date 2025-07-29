using WPFTest.MVVM.Model.Subject;

namespace WPFTest.MVVM.Model.Exercise
{
    public class FullExercise
    {
        public int Id { get; set; }
        public int? Number { get; set; }
        public string? Task {  get; set; }
        public int SubjectId { get; set; }
        public LightSubject? Subject { get; set; }
        public bool? IsLiked { get; set; }
    }
}
