using System.Windows.Input;
using WPFTest.ApiServices;
using WPFTest.Core;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Exercise;
using WPFTest.MVVM.ViewModel.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class SubjectViewModel : ObserverItem, ISubjectViewModel
    {
        private readonly ApiExerciseService _exerciseService;
        private readonly ApiSubjectService _subjectService;

        private readonly IMainViewModel _mainViewModel;


        private string? _name = string.Empty;
        private int? _year = null;
        private string? _description = string.Empty;
        private ICollection<LightExercise>? _exercises;

        public ICommand ExerciseViewCommand { get; set; }
        public ICommand ChangeIsLikedCommand { get; set; }

        public SubjectViewModel(ApiSubjectService subjectService, IMainViewModel mainViewModel, ApiExerciseService exerciseService)
        {
            _mainViewModel = mainViewModel;

            _subjectService = subjectService;
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
            try
            {
                var subject = await _subjectService.GetByIdAsync(id);

                Name = subject?.Name ?? string.Empty;
                Year = subject?.Year ?? 0;
                Description = subject?.Description ?? string.Empty;
                Exercises = subject?.Exercises ?? null;
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
    }
}
