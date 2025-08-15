using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFTest.ApiServices;
using WPFTest.Core;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Exercise;
using WPFTest.MVVM.ViewModel.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class PersonViewModel : ObserverItem, IPersonViewModel
    {

        private readonly ApiPersonService _personService;
        private readonly ApiExerciseService _exerciseService;

        private readonly IMainViewModel _mainViewModel;
        private readonly Lazy<IExerciseViewModel> _exerciseViewModel;
        private readonly Lazy<IErrorViewModel> _errorViewModel;

        private string? _id;
        private string? _name;
        private byte[]? _image;
        private ObservableCollection<LightExercise>? _exercises;

        public ICommand ChangeIsLikedCommand { get; set; }
        public ICommand ExerciseViewCommand { get; set; }

        public PersonViewModel(ApiPersonService personService, ApiExerciseService exerciseService, Lazy<IExerciseViewModel> exerciseViewModel, Lazy<IErrorViewModel> errorViewModel, IMainViewModel mainViewModel)
        {
            _personService = personService;
            _exerciseService = exerciseService;

            _exerciseViewModel = exerciseViewModel;
            _errorViewModel = errorViewModel;
            _mainViewModel = mainViewModel;

            ChangeIsLikedCommand = new AsyncRelayCommand(async x => await _exerciseService.ChangeIsLikedAsync((int)x));
            ExerciseViewCommand = new RelayCommand(x => OpenExerciseById((int)x));
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

        public async void LoadPerson(string name)
        {
            try
            {
                var person = await _personService.GetPersonByNameAsync(name);

                if (person == null)
                {
                    return;
                }

                Id = person.Id;
                Name = person.Name;
                Image = person.Image;
                Exercises = new ObservableCollection<LightExercise>(person.Exercises);
            }
            catch (ApiExeption ex)
            {
                _errorViewModel.Value.LoadError(ex.Message);
                _mainViewModel.ChangeCurrentView(_errorViewModel.Value);
            }
        }

        public void OpenExerciseById(int id)
        {
            _exerciseViewModel.Value.LoadExercise(id);
            _mainViewModel.ChangeCurrentView(_exerciseViewModel.Value);
        }
    }
}
