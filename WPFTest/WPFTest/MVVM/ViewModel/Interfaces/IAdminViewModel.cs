using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFTest.Data;
using WPFTest.MVVM.Model.Person;

namespace WPFTest.MVVM.ViewModel.Interfaces
{
    public interface IAdminViewModel
    {
        ICommand GetCommentsCommand { get; }
        ICommand ChangeRoleCommand { get; }
        ICommand UpdateUserCommand { get; }
        ICommand DeleteUserCommand { get; }
        ICommand DeleteCommentCommand { get; }

        ObservableCollection<PersonBorderData> Users { get; set; }

        Task LoadUsers();
    }
}
