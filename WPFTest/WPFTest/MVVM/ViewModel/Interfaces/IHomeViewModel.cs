namespace WPFTest.MVVM.ViewModel.Interfaces
{
    public interface IHomeViewModel
    {
        public void LoadPerson();
        void OpenExerciseById(int id);
        Task Logout();
        public Task DeleteImage();
        public Task ChangeImage();
    }
}
