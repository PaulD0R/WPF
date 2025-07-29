using System.Windows.Input;
using WPFTest.ApiServices;
using WPFTest.Core;
using WPFTest.Data;
using WPFTest.MVVM.Model.Person;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.Services.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class SignupViewModel : ObserverItem, ISignupViewModel
    {
        private readonly ApiAuthenticationService _personService;
        private readonly INavigationService _navigationService;

        private string? _name;
        private string? _email;
        private string? _password;

        public ICommand NameCommand { get; set; }
        public ICommand EmailCommand { get; set; }
        public ICommand PasswordCommand { get; set; }
        public ICommand SignupCommand { get; set; }

        public SignupViewModel(ApiAuthenticationService personService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _personService = personService;

            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;

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

        public async Task Signup()
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
        }
    }
}
