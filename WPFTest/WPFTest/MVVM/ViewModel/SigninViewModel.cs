using System.Windows.Input;
using WPFTest.ApiServices;
using WPFTest.Core;
using WPFTest.Data;
using WPFTest.MVVM.Model.Person;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.Services.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class SigninViewModel : ObserverItem, ISigninViewModel
    {
        private readonly ApiAuthenticationService _personService;
        private readonly INavigationService _navigationService;

        private string? _name = "string";
        private string? _password = "stringsT8.";

        public ICommand NameCommand { get; set; }
        public ICommand PasswordCommand { get; set; }
        public ICommand SigninCommand { get; set; }

        public SigninViewModel(ApiAuthenticationService personService, INavigationService navigationService)
        {
            _personService = personService;
            _navigationService = navigationService;

            //Name = string.Empty;
            //Password = string.Empty;

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
            }
        }

        public async Task Signin()
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
    }
}
