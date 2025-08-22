using System.Windows.Input;
using WPFTest.MVVM.Model.Subject;

namespace WPFTest.MVVM.ViewModel.Interfaces
{
    public interface INewExerciseViewModel
    {
        int? SubjectId { get; set; }
        int? Number { get; set; }
        string? Task { get; set; }
        byte[]? File { get; set; }
        ICollection<LightSubject>? Subjects { get; set; }
        bool? IsError { get; set; }
        string? ErrorText { get; set; }

        ICommand FileCommand { get; }
        ICommand SaveCommand { get; }
        ICommand NewSubjectViewCommand { get; }
    }
}
