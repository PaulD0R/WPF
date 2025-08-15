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
        private readonly ApiAuthenticationService _personService;

        private readonly INavigationService _navigationService;
        private readonly ICheckCorrectServise _checkCorrectServise;

        private string? _name;
        private string? _email;
        private string? _password;
        private bool? _isError;
        private string? _errorText;

        public ICommand NameCommand { get; set; }
        public ICommand EmailCommand { get; set; }
        public ICommand PasswordCommand { get; set; }
        public ICommand SignupCommand { get; set; }

        public SignupViewModel(ApiAuthenticationService personService, INavigationService navigationService, ICheckCorrectServise checkCorrectServise)
        {
            _navigationService = navigationService;
            _checkCorrectServise = checkCorrectServise;

            _personService = personService;

            _name = string.Empty;
            _email = string.Empty;
            _password = string.Empty;
            _isError = false;
            _errorText = string.Empty;

            NameCommand = new RelayCommand(x => Name = (string)x);
            EmailCommand = new RelayCommand(x => Email = (string)x);
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
                    var person = await _personService.Signup(signupPerson);

                    if (person != null)
                    {
                        StaticData.TOKEN = person.Token ?? string.Empty;
                        _navigationService.ShowWindow<MainWindow>();
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
