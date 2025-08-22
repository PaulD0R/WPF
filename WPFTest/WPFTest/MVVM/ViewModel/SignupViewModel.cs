using System.Windows.Input;
using WPFTest.ApiServices;
using WPFTest.Core;
using WPFTest.Data;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Person;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.Services;
using WPFTest.Services.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class SignupViewModel : ObserverItem, ISignupViewModel
    {
        private readonly ApiAuthenticationService _authenticationService;
        private readonly ICheckCorrectServise _checkCorrectServise;
        private readonly IWindowNavigationService _windowNavigationService;
        private readonly INavigationService _navigationService;

        private string? _name = string.Empty;
        private string? _email = string.Empty;
        private string? _password = string.Empty;
        private bool? _isError = false;
        private string? _errorText = string.Empty;
        private bool? _isntBlock = true;

        public ICommand PasswordCommand { get; }
        public ICommand SignupCommand { get; }

        public SignupViewModel(ApiAuthenticationService authenticationService, ICheckCorrectServise checkCorrectServise, 
            IWindowNavigationService windowNavigationService, INavigationService navigationService)
        {
            _checkCorrectServise = checkCorrectServise;
            _windowNavigationService = windowNavigationService;
            _authenticationService = authenticationService;
            _navigationService = navigationService;

            PasswordCommand = new RelayCommand(x => Password = (string)x);
            SignupCommand = new AsyncRelayCommand(async _ => await Signup());
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

        public string? Email
        {
            get => _email;
            set
            {
                _email = value;
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
                _errorText= value;
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

        private async Task Signup()
        {
            try
            {
                ErrorText = string.Empty;
                IsError = false;

                if (!_checkCorrectServise.IsEmail(Email ?? string.Empty))
                {
                    ErrorText = "Некорректная почта";
                    IsError = true;
                    return;
                }
                if (!_checkCorrectServise.IsPassword(Password ?? string.Empty))
                {
                    ErrorText = "Некорректный пароль";
                    IsError = true;
                    return;
                }
                if (Name != string.Empty && Email != string.Empty && Password != string.Empty)
                {
                    var signupPerson = new SignupPerson
                    {
                        Name = Name,
                        Email = Email,
                        Password = Password
                    };

                    IsntBlock = false;
                    var token = await _authenticationService.Signup(signupPerson);
                    IsntBlock = true;

                    if (token != null)
                    {
                        Name = string.Empty;
                        Email = string.Empty;
                        Password = string.Empty;

                        StaticData.TOKEN = token.Jwt ?? string.Empty;
                        TokenStorageService.SaveRefreshToken(token.RefreshToken ?? string.Empty);

                        _navigationService.ClearHistory();
                        _windowNavigationService.ShowAndHideAnotherWindow<MainWindow>();
                    }
                    else
                    {
                        ErrorText = "Ошибка";
                        IsError = true;
                    }
                }
                else
                {
                    ErrorText = "Заполните все поля";
                    IsError = true;
                }
            }
            catch (ApiExeption ex)
            {
                ErrorText = ex.Message;
                IsError = true;
            }
            catch
            {
                ErrorText = "Неизвестная ошибка";
                IsError = true;
            }
        }
    }
}
