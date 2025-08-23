using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFTest.MVVM.Model.Exercise;

namespace WPFTest.MVVM.ViewModel.Interfaces
{
    public interface IPersonViewModel
    {
        string? Id { get; set; }
        string? Name { get; set; }
        byte[]? Image { get; set; }
        ObservableCollection<LightExercise>? Exercises { get; set; }

        ICommand ChangeIsLikedCommand { get; }
        ICommand ExerciseViewCommand { get; }

        public Task LoadPerson(string name);
    }
}
