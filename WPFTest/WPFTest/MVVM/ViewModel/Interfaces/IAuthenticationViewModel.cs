using System.Windows.Input;
using WPFTest.MVVM.Model.Person;

namespace WPFTest.MVVM.ViewModel.Interfaces
{
    public interface IAuthenticationViewModel
    {
        ICommand SigninCommand { get; }
        ICommand SignupCommand { get; }
    }
}
