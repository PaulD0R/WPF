using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFTest.Data;
using WPFTest.MVVM.Model.Exercise;

namespace WPFTest.MVVM.ViewModel.Interfaces
{
    public interface IDiscoverViewModel
    {
        ObservableCollection<LightExercise> Exercises { get; set; }
        List<PageButtonData> Pages {  get; set; }
        int PageNumber {  get; set; }

        ICommand ChangePageCommand { get; }
        ICommand ExerciseViewCommand { get; }
        ICommand ChangeIsLikedCommand { get; }

        void LoadPages();
        void LoadExercises();
    }
}
