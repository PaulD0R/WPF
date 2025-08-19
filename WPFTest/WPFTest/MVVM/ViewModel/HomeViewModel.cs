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

        private readonly IMainViewModel _mainViewModel;

        private readonly INavigationService _navigationService;

        private string? _id = string.Empty;
        private string? _name = string.Empty;
        private string? _email = string.Empty;
        private byte[]? _image = [];
        private ObservableCollection<LightExercise>? _exercises = [];

        public ICommand ChangeImageCommand { get; set; }
        public ICommand DeleteImageCommand { get; set; }
        public ICommand ChangeIsLikedCommand { get; set; }
        public ICommand ExerciseViewCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        public HomeViewModel(ApiPersonService personService, ApiExerciseService exerciseService, ApiAuthenticationService authenticationService, INavigationService navigationService, IMainViewModel mainViewModel)
        {
            _personService = personService;
            _exerciseService = exerciseService;
            _authenticationService = authenticationService;

            _navigationService = navigationService;

            _mainViewModel = mainViewModel;

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

        public async Task ChangeImage()
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
                _mainViewModel.OpenErrorView(ex.Message);
            }
        }

        public async Task DeleteImage()
        {
            try
            {
                if (await _personService.DeletePrivateImageAsync())
                    Image = null;
            }
            catch (ApiExeption ex)
            {
                _mainViewModel.OpenErrorView(ex.Message);
            }
        }

        public async void LoadPerson()
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
                _mainViewModel.OpenErrorView(ex.Message);
            }
        }

        public void OpenExerciseById(int id)
        {
            try
            {
                 _mainViewModel.OpenExerciseView(id);
            }
            catch (ApiExeption ex)
            {
                _mainViewModel.OpenErrorView(ex.Message);
            }
        }

        public async Task Logout()
        {
            try
            {
                if (await _authenticationService.Logout())
                {
                    TokenStorageService.ClearRefreshToken();
                    StaticData.TOKEN = string.Empty;

                    _navigationService.ShowAndClothesAnotherWindow<AuthenticationWindow>();
                }
            }
            catch (ApiExeption ex)
            {
                _mainViewModel.OpenErrorView(ex.Message);
            }
        }
    }
}
