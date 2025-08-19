using System.Windows.Input;
using WPFTest.ApiServices;
using WPFTest.Core;
using WPFTest.Data;
using WPFTest.MVVM.Model.Person;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.Services;
using WPFTest.Services.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class AuthenticationViewModel : ObserverItem, IAuthenticationViewModel
    {
        private readonly ApiAuthenticationService _authenticationServixe;

        private readonly INavigationService _navigationService;

        private object? _curentView = null;
        private bool _isntBlock = true;

        private readonly Lazy<ISigninViewModel> _signinViewModel;
        private readonly Lazy<ISignupViewModel> _signupViewModel;

        public ICommand SigninCommand { get; set; }
        public ICommand SignupCommand { get; set; }


        public AuthenticationViewModel(Lazy<ISigninViewModel> signinVewModel, Lazy<ISignupViewModel> signupViewModel, ApiAuthenticationService authenticationService, INavigationService navigationService)
        {
            _signinViewModel = signinVewModel;
            _signupViewModel = signupViewModel;

            _navigationService = navigationService;

            _authenticationServixe = authenticationService;

            SigninCommand = new RelayCommand(_ => ChangeCurrentView(_signinViewModel.Value));
            SignupCommand = new RelayCommand(_ => ChangeCurrentView(_signupViewModel.Value));

            _navigationService.CloseAnotherWindow<AuthenticationWindow>();

            CheckToken();
        }

        public object? CurentView
        {
            get => _curentView;
            set
            {
                _curentView = value;
                OnPropertyChanged();
            }
        }

        public bool IsntBlock
        {
            get => _isntBlock;
            set
            {
                _isntBlock = value;
                OnPropertyChanged();
            }
        }

        public void OpenMainApplication(Token tokens)
        {
            try
            {
                if (tokens == null) return;

                StaticData.TOKEN = tokens.Jwt ?? string.Empty;
                TokenStorageService.SaveRefreshToken(tokens.RefreshToken ?? string.Empty);

                _navigationService.ShowAndClothesAnotherWindow<MainWindow>();
            }
            catch
            {
                return;
            }
        }

        public void ChangeCurrentView(object curentView)
        {
            CurentView = curentView;
        }

        public async void CheckToken()
        {
            try
            {
                var token = TokenStorageService.LoadRefreshToken();

                if (string.IsNullOrEmpty(token)) return;

                var tokens = await _authenticationServixe.SigninWithToken(token);

                if (tokens == null) return;

                StaticData.TOKEN = tokens.Jwt ?? string.Empty;
                TokenStorageService.SaveRefreshToken(tokens.RefreshToken ?? string.Empty);

                _navigationService.ShowAndClothesAnotherWindow<MainWindow>();
            } catch
            {
                return;
            }
        }
    }
}
