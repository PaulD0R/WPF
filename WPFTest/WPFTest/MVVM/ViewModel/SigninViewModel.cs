using System.Security;
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
    public class SigninViewModel : ObserverItem, ISigninViewModel
    {
        private readonly ApiAuthenticationService _personService;

        private readonly INavigationService _navigationService;
        private readonly ICheckCorrectServise _checkCorrectServise;

        private string? _name = string.Empty;
        private string? _password = string.Empty;
        private bool? _isError = false;
        private string? _errorText = string.Empty;

        public ICommand NameCommand { get; set; }
        public ICommand PasswordCommand { get; set; }
        public ICommand SigninCommand { get; set; }

        public SigninViewModel(ApiAuthenticationService personService, INavigationService navigationService, ICheckCorrectServise checkCorrectServise)
        {
            _personService = personService;

            _navigationService = navigationService;
            _checkCorrectServise = checkCorrectServise;

            NameCommand = new RelayCommand(x => Name = (string)x);
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

        public async Task Signin()
        {
            try
            {
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

                    var person = await _personService.Signin(signinPerson);

                    if (person != null)
                    {
                        StaticData.TOKEN = person.Token ?? string.Empty;
                        _navigationService.ShowWindow<MainWindow>();
                    }
                }
                else
                {
                    ErrorText = "Заполните все поля";
                    IsError = true;
                    return;
                }
            }
            catch (ApiExeption ex)
            {
                ErrorText = ex.Message;
                IsError = true;
            }
            catch
            {
                ErrorText = "Ошибка";
                IsError = true;
            }
        }
    }
}
