using System.Windows.Input;
using WPFTest.ApiServices;
using WPFTest.Core;
using WPFTest.Data;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Person;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.Services.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class SignupViewModel : ObserverItem, ISignupViewModel
    {
        private readonly ApiAuthenticationService _authenticationService;
        private readonly ICheckCorrectServise _checkCorrectServise;

        private string? _name = string.Empty;
        private string? _email = string.Empty;
        private string? _password = string.Empty;
        private bool? _isError = false;
        private string? _errorText = string.Empty;
        private bool? _isntBlock = true;

        private readonly Lazy<IAuthenticationViewModel> _authenticationViewModel;

        public ICommand PasswordCommand { get; set; }
        public ICommand SignupCommand { get; set; }

        public SignupViewModel(ApiAuthenticationService authenticationService, ICheckCorrectServise checkCorrectServise, Lazy<IAuthenticationViewModel> authenticationViewModel)
        {
            _checkCorrectServise = checkCorrectServise;
            _authenticationViewModel = authenticationViewModel;

            _authenticationService = authenticationService;

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

        public async Task Signup()
        {
            try
            {
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

                        _authenticationViewModel.Value.OpenMainApplication(token);
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
