namespace WPFTest.MVVM.ViewModel.Interfaces
{
    public interface IMainViewModel
    {
        void OpenPersonView();
        void OpenExerciseView(int id);
        void OpenNewExercise();
        void OpenNewSubjectView();
        void OpenHomeView();
        void OpenDictonaryView();
        void OpenErrorView(string error);
        void OpenSubject(int id);
        void LoadRoles();
    }
}
