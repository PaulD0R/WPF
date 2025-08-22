using System.Windows.Input;

namespace WPFTest.MVVM.ViewModel.Interfaces
{
    public interface ISigninViewModel
    {
        string? Name { get; set; }
        string? Password { get; set; }
        bool? IsError { get; set; }
        string? ErrorText { get; set; }
        bool? IsntBlock { get; set; }

        ICommand PasswordCommand { get; }
        ICommand SigninCommand { get; }
    }
}
