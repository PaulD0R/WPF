namespace WPFServer.Models
{
    public class ExercisesFiles
    {
        public int Id { get; set; }
        public byte[] TasksFile { get; set; } = [];
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; } =  null!;
    }
}
