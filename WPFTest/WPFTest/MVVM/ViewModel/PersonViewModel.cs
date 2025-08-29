using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFTest.ApiServices.Interfaces;
using WPFTest.Core;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Exercise;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.Services.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class PersonViewModel : ObserverItem, IPersonViewModel
    {

        private readonly IApiPersonService _personService;
        private readonly IApiExerciseService _exerciseService;
        private readonly INavigationService _navigationService;

        private string? _id = null;
        private string? _name = string.Empty;
        private byte[]? _image = [];
        private ObservableCollection<LiteExercise>? _exercises;

        public ICommand ChangeIsLikedCommand { get; }
        public ICommand ExerciseViewCommand { get; }

        public PersonViewModel(IApiPersonService personService, IApiExerciseService exerciseService, 
            INavigationService navigationService)
        {
            _personService = personService;
            _exerciseService = exerciseService;
            _navigationService = navigationService;

            ChangeIsLikedCommand = new AsyncRelayCommand(async x => await ChangeIsLiked((int)x));
            ExerciseViewCommand = new AsyncRelayCommand(async x => await OpenExerciseById((int)x));
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

        public ObservableCollection<LiteExercise>? Exercises
        {
            get => _exercises;
            set
            {
                _exercises = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadPerson(string name)
        {
            var person = await _personService.GetPersonByNameAsync(name);

            if (person == null)
            {
                return;
            }

            Id = person.Id;
            Name = person.Name;
            Image = person.Image;
            Exercises = new ObservableCollection<LiteExercise>(person.Exercises ?? []);
        }

        private async Task OpenExerciseById(int id)
        {
            try
            {
                await _navigationService.NavigateToAsync<IExerciseViewModel>(async x => await x.LoadExercise(id));
            }
            catch (ApiException ex)
            {
                _navigationService.NavigateTo<IErrorViewModel>(x => x.LoadError(ex.Message));
            }
        }

        private async Task ChangeIsLiked(int id)
        {
            try
            {
                await _exerciseService.ChangeIsLikedAsync(id);
            }
            catch
            {
                return;
            }
        }
    }
}
