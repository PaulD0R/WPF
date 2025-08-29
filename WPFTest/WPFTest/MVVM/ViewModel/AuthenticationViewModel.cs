using System.Windows.Input;
using WPFTest.ApiServices.Interfaces;
using WPFTest.Core;
using WPFTest.Data;
using WPFTest.Exeptions;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.Services;
using WPFTest.Services.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class AuthenticationViewModel : ObserverItem, IAuthenticationViewModel, IDisposable
    {
        private readonly IApiAuthenticationService _authenticationServixe;
        private readonly IWindowNavigationService _windowNvigationService;
        private readonly IModelNavigationService _modelNavigationService;

        public ICommand SigninCommand { get; }
        public ICommand SignupCommand { get; }

        public AuthenticationViewModel(IApiAuthenticationService authenticationService, 
            IWindowNavigationService windowNavigationService, IModelNavigationService modelNavigationService)
        {
            _windowNvigationService = windowNavigationService;
            _modelNavigationService = modelNavigationService;
            _authenticationServixe = authenticationService;

            CurrentView = _modelNavigationService.CurrentView;

            SigninCommand = new RelayCommand(_ => OpenSignin());
            SignupCommand = new RelayCommand(_ => OpenSignup());

            _modelNavigationService.NavigationChanged += ChangeCurrentView;

            CheckToken();
        }

        public object? CurrentView {get; set;}

        private void OpenSignin()
        {
            try
            {
                _modelNavigationService.NavigateTo<ISigninViewModel>();
            } 
            catch (ApiException ex)
            {
                _modelNavigationService.NavigateTo<IErrorViewModel>();
            }
        }

        private void OpenSignup()
        {
            try
            {
                _modelNavigationService.NavigateTo<ISignupViewModel>();
            }
            catch (ApiException ex)
            {
                _modelNavigationService.NavigateTo<IErrorViewModel>();
            }
        }

        private void ChangeCurrentView(object sender, object value)
        {
            CurrentView = _modelNavigationService.CurrentView;
            OnPropertyChanged(nameof(CurrentView));
        }

        private async void CheckToken()
        {
            try
            {
                var token = TokenStorageService.LoadRefreshToken();

                if (string.IsNullOrEmpty(token)) return;

                var tokens = await _authenticationServixe.SigninWithToken(token);

                if (tokens == null) return;

                StaticData.TOKEN = tokens.Jwt ?? string.Empty;
                TokenStorageService.SaveRefreshToken(tokens.RefreshToken ?? string.Empty);

                _windowNvigationService.ShowAndHideAnotherWindow<MainWindow>();
            } 
            catch
            {
                return;
            }
        }

        public void Dispose()
        {
            _modelNavigationService.NavigationChanged -= ChangeCurrentView;
        }
    }
}
