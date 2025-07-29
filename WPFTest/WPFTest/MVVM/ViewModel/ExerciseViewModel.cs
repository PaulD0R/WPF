

using System.Windows.Input;
using WPFTest.ApiServices;
using WPFTest.Core;
using WPFTest.FileStreamers;
using WPFTest.MVVM.ViewModel.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class ExerciseViewModel : ObserverItem, IExerciseViewModel
    {
        private readonly ApiExerciseService _exerciseService;
        private readonly ApiPersonService _personService;

        private readonly IMainViewModel _mainViewModel;
        private readonly Lazy<ISubjectViewModel> _subjectViewModel;

        private int _id = 0;
        private int _subjectId = 0;
        private string? _subject = null;
        private int? _year = null;
        private int? _number = null;
        private string? _task = null;
        private bool _isLoaded = false;
        private bool? _isLiked = false;
        private int? _likesCount = null;

        public ICommand SubjectViewCommand { get; set; }
        public ICommand LoadTasksFileCommand { get; set; }
        public ICommand ChangeIsLikedCommand { get; set; }

        public ExerciseViewModel(ApiExerciseService apiExerciseService, ApiPersonService personService, IMainViewModel mainViewModel, Lazy<ISubjectViewModel> subjectViewModel)
        {
            _exerciseService = apiExerciseService;
            _personService = personService;

            _mainViewModel = mainViewModel;
            _subjectViewModel = subjectViewModel;

            SubjectViewCommand = new RelayCommand(_ => OpenSubjectById(SubjectId));
            LoadTasksFileCommand = new AsyncRelayCommand(async _ => await GetExercisesTasksFileAsync());
            ChangeIsLikedCommand = new AsyncRelayCommand(async x =>
            {
                await _exerciseService.ChangeIsLikedAsync((int)x);
                LikesCount = await GetLikesCount();
            });
        }

        public int Id 
        {
            get => _id;
            set => _id = value;
        }

        public int SubjectId
        {
            get => _subjectId;
            set => _subjectId = value;
        }

        public string? Subject
        {
            get => _subject;
            set
            {
                _subject = value;
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

        public int? Number 
        {
            get => _number;
            set
            {
                _number = value;
                OnPropertyChanged();
            }
        }

        public string? Task
        {
            get => _task;
            set
            {
                _task = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoaded
        {
            get => _isLoaded;
            set
            {
                _isLoaded = value;
                OnPropertyChanged();
            }
        }

        public bool? IsLiked
        {
            get => _isLiked;
            set
            {
                _isLiked = value;
                OnPropertyChanged();
            }
        }

        public int? LikesCount 
        {
            get => _likesCount;
            set
            {
                _likesCount = value;
                OnPropertyChanged();
            }
        }

        public async void LoadExercise(int id)
        {
            IsLoaded = false;
            Id = id;
            var exercise = await _exerciseService.GetByIdAsync(Id);

            if (exercise == null) throw new Exception();

            Id = exercise.Id;
            SubjectId = exercise.SubjectId;
            Subject = exercise.Subject?.Name;
            Year = exercise.Subject?.Year;
            Number = exercise.Number;
            Task = exercise.Task;
            IsLoaded = true;
            IsLiked = await _personService.GetIsLickedAsync(Id);
            LikesCount = await GetLikesCount();
        }

        public async Task<int?> GetLikesCount()
        {
            var exerciseState = await _exerciseService.GetLikesCountByIdAsync(Id);

            if (exerciseState == null) return null;

            return exerciseState.LikesCount;
        }

        public void OpenSubjectById(int id)
        {
            if (id <= 0) return;

            (_subjectViewModel.Value).LoadSubject(id);
            _mainViewModel.ChangeCurrentView(_subjectViewModel.Value);
        }

        public async Task GetExercisesTasksFileAsync()
        {
            var files = await _exerciseService.GetFileByIdAsync(Id);
            if (files == null) throw new Exception();

            ZipFileStreamer.GetFile(files.TasksFile ?? []);
        }
    }
}
