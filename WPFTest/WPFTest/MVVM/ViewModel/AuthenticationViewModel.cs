using System.Windows.Input;
using WPFTest.Core;
using WPFTest.MVVM.ViewModel.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class AuthenticationViewModel : ObserverItem, IAuthenticationViewModel
    {
        private object _curentView;

        public object SigninViewModel { get; set; }
        public object SignupViewModel { get; set; }

        public ICommand SigninCommand { get; set; }
        public ICommand SignupCommand { get; set; }


        public AuthenticationViewModel(ISignupViewModel signinVewModel, ISigninViewModel signupViewModel)
        {
            SigninViewModel = signinVewModel;
            SignupViewModel = signupViewModel;

            _curentView = SigninViewModel;

            SigninCommand = new RelayCommand(_ => CurentView = SigninViewModel);
            SignupCommand = new RelayCommand(_ => CurentView = SignupViewModel);
        }

        public object CurentView
        {
            get => _curentView;
            set
            {
                _curentView = value;
                OnPropertyChanged();
            }
        }
    }
}
