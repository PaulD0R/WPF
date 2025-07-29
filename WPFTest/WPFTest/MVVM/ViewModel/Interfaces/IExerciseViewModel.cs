namespace WPFTest.MVVM.ViewModel.Interfaces
{
    public interface IExerciseViewModel
    {
        void LoadExercise(int id);
        void OpenSubjectById(int id);
        public Task GetExercisesTasksFileAsync();
    }
}
