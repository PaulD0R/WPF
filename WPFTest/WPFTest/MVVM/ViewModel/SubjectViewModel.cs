using System.Windows.Input;
using WPFTest.ApiServices.Interfaces;
using WPFTest.Core;
using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Exercise;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.Services.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class SubjectViewModel : ObserverItem, ISubjectViewModel
    {
        private readonly IApiExerciseService _exerciseService;
        private readonly IApiSubjectService _subjectService;
        private readonly INavigationService _navigationService;


        private string? _name = string.Empty;
        private int? _year = null;
        private string? _description = string.Empty;
        private ICollection<LiteExercise>? _exercises;

        public ICommand ExerciseViewCommand { get; }
        public ICommand ChangeIsLikedCommand { get; }

        public SubjectViewModel(IApiSubjectService subjectService, IApiExerciseService exerciseService,
            INavigationService navigationService)
        {
            _subjectService = subjectService;
            _exerciseService = exerciseService;
            _navigationService = navigationService;

            ExerciseViewCommand = new AsyncRelayCommand(async x => await OpenExerciseById((int)x));
            ChangeIsLikedCommand = new AsyncRelayCommand(async x => await ChangeIsLiked((int)x));
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

        public ICollection<LiteExercise>? Exercises
        {
            get => _exercises;
            set
            {
                _exercises = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadSubject(int id)
        {
            var subject = await _subjectService.GetByIdAsync(id);

            Name = subject?.Name ?? string.Empty;
            Year = subject?.Year ?? 0;
            Description = subject?.Description ?? string.Empty;
            Exercises = subject?.Exercises ?? null;
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
