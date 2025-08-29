using System.Windows.Input;
using WPFTest.ApiServices.Interfaces;
using WPFTest.Core;
using WPFTest.Data;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Person;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.Services;
using WPFTest.Services.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class SigninViewModel : ObserverItem, ISigninViewModel
    {
        private readonly IApiAuthenticationService _authenticationService;
        private readonly ICheckCorrectService _checkCorrectServise;
        private readonly IWindowNavigationService _windowNavigationService;
        private readonly INavigationService _navigationService;

        private string? _name = string.Empty;
        private string? _password = string.Empty;
        private bool? _isError = false;
        private string? _errorText = string.Empty;
        private bool? _isntBlock = true;

        public ICommand PasswordCommand { get; }
        public ICommand SigninCommand { get; }

        public SigninViewModel(IApiAuthenticationService authenticationService, ICheckCorrectService checkCorrectServise, 
            IWindowNavigationService windowNavigationService, INavigationService navigationService)
        {
            _authenticationService = authenticationService;
            _checkCorrectServise = checkCorrectServise;
            _windowNavigationService = windowNavigationService;
            _navigationService = navigationService;

            PasswordCommand = new RelayCommand(x => Password = (string)x);
            SigninCommand = new AsyncRelayCommand(async _ => await Signin());
        }

        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string? Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public bool? IsError
        {
            get => _isError;
            set
            {
                _isError = value;
                OnPropertyChanged();
            }
        }

        public string? ErrorText 
        {
            get => _errorText;
            set
            {
                _errorText = value;
                OnPropertyChanged();
            }
        }

        public bool? IsntBlock
        {
            get => _isntBlock;
            set
            {
                _isntBlock = value;
                OnPropertyChanged();
            }
        }

        private async Task Signin()
        {
            try
            {
                ErrorText = string.Empty;
                IsError = false;

                if (!_checkCorrectServise.IsPassword(Password ?? string.Empty))
                {
                    ErrorText = "Некорректный пароль";
                    IsError = true;
                    return;
                }
                if (Name != string.Empty && Password != string.Empty)
                {
                    var signinPerson = new SigninPerson
                    {
                        Name = Name,
                        Password = Password
                    };

                    IsntBlock = false;
                    var token = await _authenticationService.Signin(signinPerson);
                    IsntBlock = true;

                    if (token != null)
                    {
                        Name = string.Empty;
                        Password = string.Empty;

                        StaticData.TOKEN = token.Jwt ?? string.Empty;
                        TokenStorageService.SaveRefreshToken(token.RefreshToken ?? string.Empty);

                        _navigationService.ClearHistory();
                        _windowNavigationService.ShowAndHideAnotherWindow<MainWindow>();
                    }
                }
                else
                {
                    ErrorText = "Заполните все поля";
                    IsError = true;
                    return;
                }
            }
            catch (ApiException ex)
            {
                ErrorText = ex.Message;
                IsError = true;
                IsntBlock = true;
            }
            catch
            {
                ErrorText = "Ошибка";
                IsError = true;
                IsntBlock = true;
            }
        }
    }
}
