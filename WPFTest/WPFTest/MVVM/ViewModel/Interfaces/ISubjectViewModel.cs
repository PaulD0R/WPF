using System.Windows.Input;
using WPFTest.MVVM.Model.Exercise;

namespace WPFTest.MVVM.ViewModel.Interfaces
{
    public interface ISubjectViewModel
    {
        string? Name { get; set; }
        int? Year { get; set; }
        string? Description { get; set; }
        ICollection<LiteExercise>? Exercises { get; set; }

        ICommand ExerciseViewCommand { get; }
        ICommand ChangeIsLikedCommand { get; }

        Task LoadSubject(int id);
    }
}
