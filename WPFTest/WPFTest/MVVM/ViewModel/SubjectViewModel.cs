using System.Windows.Input;
using WPFTest.ApiServices;
using WPFTest.Core;
using WPFTest.MVVM.Model.Exercise;
using WPFTest.MVVM.ViewModel.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class SubjectViewModel : ObserverItem, ISubjectViewModel
    {
        private readonly ApiExerciseService _exerciseService;

        private readonly IMainViewModel _mainViewModel;
        private readonly Lazy<IExerciseViewModel> _exerciseViewModel;
        private readonly ApiSubjectService _subjectService;

        private string? _name;
        private int? _year;
        private string? _description;
        private ICollection<LightExercise>? _exercises;

        public ICommand ExerciseViewCommand { get; set; }
        public ICommand ChangeIsLikedCommand { get; set; }

        public SubjectViewModel(ApiSubjectService subjectService, IMainViewModel mainViewModel, Lazy<IExerciseViewModel> exerciseViewModel, ApiExerciseService exerciseService)
        {
            _mainViewModel = mainViewModel;
            _subjectService = subjectService;
            _exerciseViewModel = exerciseViewModel;
            _exerciseService = exerciseService;

            ExerciseViewCommand = new RelayCommand(x => OpenExerciseById((int)x));
            ChangeIsLikedCommand = new AsyncRelayCommand(async x => await _exerciseService.ChangeIsLikedAsync((int)x));
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

        public int? Year
        {
            get => _year;
            set
            {
                _year = value;
                OnPropertyChanged();
            }
        }

        public string? Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public ICollection<LightExercise>? Exercises
        {
            get => _exercises;
            set
            {
                _exercises = value;
                OnPropertyChanged();
            }
        }

        public async void LoadSubject(int id)
        {
            var subject = await _subjectService.GetByIdAsync(id);

            Name = subject?.Name ?? string.Empty;
            Year = subject?.Year ?? 0;
            Description = subject?.Description ?? string.Empty;
            Exercises = subject?.Exercises ?? null;
        }

        public void OpenExerciseById(int id)
        {
            (_exerciseViewModel.Value).LoadExercise(id);
            _mainViewModel.ChangeCurrentView(_exerciseViewModel.Value);
        }
    }
}
