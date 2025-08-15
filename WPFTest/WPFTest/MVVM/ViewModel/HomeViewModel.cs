using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFTest.ApiServices;
using WPFTest.Core;
using WPFTest.Exeptions;
using WPFTest.FileStreamers;
using WPFTest.MVVM.Model.Exercise;
using WPFTest.MVVM.Model.Files;
using WPFTest.MVVM.ViewModel.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class HomeViewModel : ObserverItem, IHomeViewModel
    {
        private readonly ApiPersonService _personService;
        private readonly ApiExerciseService _exerciseService;

        private readonly Lazy<IMainViewModel> _mainViewModel;
        private readonly Lazy<IExerciseViewModel> _exerciseViewModel;
        private readonly Lazy<IErrorViewModel> _errorViewModel;

        private string? _id;
        private string? _name;
        private string? _email;
        private byte[]? _image;
        private ObservableCollection<LightExercise>? _exercises;

        public ICommand ChangeImageCommand { get; set; }
        public ICommand DeleteImageCommand { get; set; }
        public ICommand ChangeIsLikedCommand { get; set; }
        public ICommand ExerciseViewCommand { get; set; }

        public HomeViewModel(ApiPersonService personService, ApiExerciseService exerciseService, Lazy<IExerciseViewModel> exerciseViewModel, Lazy<IErrorViewModel> errorViewModel, Lazy<IMainViewModel> mainViewModel)
        {
            _personService = personService;
            _exerciseService = exerciseService;

            _exerciseViewModel = exerciseViewModel;
            _errorViewModel = errorViewModel;
            _mainViewModel = mainViewModel;

            ChangeImageCommand = new AsyncRelayCommand(async _ => await ChangeImage());
            DeleteImageCommand = new AsyncRelayCommand(async _ => await DeleteImage());
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
            var newImageFile = new ImageNewFile
            {
                Image = ImgFileStreamer.SetFile()
            };

            if (newImageFile.Image != null) 
                Image = (await _personService.ChangePrivateImageAsync(newImageFile))?.Image;
        }

        public async Task DeleteImage()
        {
            if (await _personService.DeletePrivateImageAsync())
                Image = null;
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
                Exercises = new ObservableCollection<LightExercise>(person.Exercises);
            }
            catch (ApiExeption ex)
            {
                _errorViewModel.Value.LoadError(ex.Message);
                _mainViewModel.Value.ChangeCurrentView(_errorViewModel.Value);
            }
        }

        public void OpenExerciseById(int id)
        {
            _exerciseViewModel.Value.LoadExercise(id);
            _mainViewModel.Value.ChangeCurrentView(_exerciseViewModel.Value);
        }
    }
}
