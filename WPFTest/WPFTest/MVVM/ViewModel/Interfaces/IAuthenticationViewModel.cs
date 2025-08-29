using System.Windows.Input;

namespace WPFTest.MVVM.ViewModel.Interfaces
{
    public interface IAuthenticationViewModel
    {
        ICommand SigninCommand { get; }
        ICommand SignupCommand { get; }
    }
}
