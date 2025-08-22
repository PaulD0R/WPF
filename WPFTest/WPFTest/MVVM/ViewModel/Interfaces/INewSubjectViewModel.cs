using System.Windows.Input;

namespace WPFTest.MVVM.ViewModel.Interfaces
{
    public interface INewSubjectViewModel
    {
        string? Name { get; set; }
        int? Year { get; set; }
        string? Description { get; set; }
        bool? IsError { get; set; }
        string? ErrorText { get; set; }

        ICommand SaveCommand { get; }
    }
}
