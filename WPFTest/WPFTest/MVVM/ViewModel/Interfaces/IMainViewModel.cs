using System.Windows.Input;

namespace WPFTest.MVVM.ViewModel.Interfaces
{
    public interface IMainViewModel
    {
        bool? IsDiscover {  get; set; }
        bool? IsHome {  get; set; }
        bool? IsAddVisible { get; set; }
        string? FindName {  get; set; }

        ICommand HomeCommand { get; }
        ICommand DiscoverCommand { get; }
        ICommand NewExercisesCommand { get; }
        ICommand FindPersonCommand { get; }
        ICommand BackCommand { get; }
    }
}
