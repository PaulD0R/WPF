namespace WPFTest.MVVM.ViewModel.Interfaces
{
    public interface INewExerciseViewModel
    {
        void LoadSubjects();
        Task CreateNewExerciseAsync();
        void OpenNewSubject();
    }
}
