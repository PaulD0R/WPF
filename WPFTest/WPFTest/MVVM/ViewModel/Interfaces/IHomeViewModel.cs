using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFTest.MVVM.Model.Exercise;

namespace WPFTest.MVVM.ViewModel.Interfaces
{
    public interface IHomeViewModel
    {
        string? Id { get; set; }
        string? Name {  get; set; }
        string? Email {  get; set; }
        byte[]? Image {  get; set; }
        ObservableCollection<LightExercise>? Exercises {  get; set; }

        ICommand ChangeImageCommand { get; }
        ICommand DeleteImageCommand { get; }
        ICommand ChangeIsLikedCommand { get; }
        ICommand ExerciseViewCommand { get; }
        ICommand LogoutCommand { get; }

        Task LoadPerson();
    }
}
