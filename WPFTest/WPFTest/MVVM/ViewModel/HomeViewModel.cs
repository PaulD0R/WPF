using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFTest.ApiServices;
using WPFTest.Core;
using WPFTest.Data;
using WPFTest.Exeptions;
using WPFTest.FileStreamers;
using WPFTest.MVVM.Model.Exercise;
using WPFTest.MVVM.Model.Files;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.Services;
using WPFTest.Services.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class HomeViewModel : ObserverItem, IHomeViewModel
    {
        private readonly ApiPersonService _personService;
        private readonly ApiExerciseService _exerciseService;
        private readonly ApiAuthenticationService _authenticationService;
        private readonly INavigationService _navigationService;
        private readonly IWindowNavigationService _windowNavigationService;

        private string? _id = string.Empty;
        private string? _name = string.Empty;
        private string? _email = string.Empty;
        private byte[]? _image = [];
        private ObservableCollection<LightExercise>? _exercises = [];

        public ICommand ChangeImageCommand { get; }
        public ICommand DeleteImageCommand { get; }
        public ICommand ChangeIsLikedCommand { get; }
        public ICommand ExerciseViewCommand { get; }
        public ICommand LogoutCommand { get; }

        public HomeViewModel(ApiPersonService personService, ApiExerciseService exerciseService, 
            ApiAuthenticationService authenticationService, IWindowNavigationService windowNavigationService, 
            INavigationService navigationService)
        {
            _personService = personService;
            _exerciseService = exerciseService;
            _authenticationService = authenticationService;
            _windowNavigationService = windowNavigationService;
            _navigationService = navigationService;

            ChangeImageCommand = new AsyncRelayCommand(async _ => await ChangeImage());
            DeleteImageCommand = new AsyncRelayCommand(async _ => await DeleteImage());
            LogoutCommand = new AsyncRelayCommand(async _ => await  Logout());
            ChangeIsLikedCommand = new AsyncRelayCommand(async x => await _exerciseService.ChangeIsLikedAsync((int)x));
            ExerciseViewCommand = new RelayCommand(x => OpenExerciseById((int)x));

            LoadPerson();
        }

        public string? Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
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

        public byte[]? Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<LightExercise>? Exercises
        {
            get => _exercises;
            set
            {
                _exercises = value;
                OnPropertyChanged();
            }
        }

        private async Task ChangeImage()
        {
            try
            {
                var newImageFile = new ImageNewFile
                {
                    Image = ImgFileStreamer.SetFile()
                };

                if (newImageFile.Image != null)
                    Image = (await _personService.ChangePrivateImageAsync(newImageFile))?.Image;
            } 
            catch (ApiExeption ex)
            {
                _navigationService.NavigateTo<IErrorViewModel>(x => x.LoadError(ex.Message));
            }
        }

        private async Task DeleteImage()
        {
            try
            {
                if (await _personService.DeletePrivateImageAsync())
                    Image = null;
            }
            catch (ApiExeption ex)
            {
                _navigationService.NavigateTo<IErrorViewModel>(x => x.LoadError(ex.Message));
            }
        }

        private void OpenExerciseById(int id)
        {
            _navigationService.NavigateTo<IExerciseViewModel>(x => x.LoadExercise(id));
        }

        private async Task Logout()
        {
            try
            {
                if (await _authenticationService.Logout())
                {
                    TokenStorageService.ClearRefreshToken();
                    StaticData.TOKEN = string.Empty;

                    _navigationService.ClearHistory();
                    _windowNavigationService.ShowAndHideAnotherWindow<AuthenticationWindow>();
                }
            }
            catch (ApiExeption ex)
            {
                _navigationService.NavigateTo<IErrorViewModel>(x => x.LoadError(ex.Message));
            }
        }

        private async void LoadPerson()
        {
            try
            {
                var person = await _personService.GetPrivateAsync();

                if (person == null)
                {
                    return;
                }

                Id = person.Id;
                Name = person.Name;
                Email = person.Email;
                Image = person.Image;
                Exercises = new ObservableCollection<LightExercise>(person.Exercises ?? []);
            }
            catch (ApiExeption ex)
            {
                _navigationService.NavigateTo<IErrorViewModel>(x => x.LoadError(ex.Message));
            }
        }
    }
}
