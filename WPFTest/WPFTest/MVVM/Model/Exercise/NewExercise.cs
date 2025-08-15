using WPFTest.MVVM.Model.Files;

namespace WPFTest.MVVM.Model.Exercise
{
    public class NewExercise
    {
        public string? Name { get; set; }
        public int? SubjectId { get; set; }
        public int? Number { get; set; }
        public string? Task {  get; set; }
        public ExercisesTasksFile? Files {  get; set; }
    }
}
