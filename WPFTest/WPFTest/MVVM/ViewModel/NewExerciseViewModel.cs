using System.Windows.Input;
using WPFTest.ApiServices;
using WPFTest.Core;
using WPFTest.FileStreamers;
using WPFTest.MVVM.Model.Exercise;
using WPFTest.MVVM.Model.Files;
using WPFTest.MVVM.Model.Subject;
using WPFTest.MVVM.ViewModel.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class NewExerciseViewModel : ObserverItem, INewExerciseViewModel
    {
        private readonly ApiExerciseService _exerciseService;
        private readonly ApiSubjectService _subjectService;
        private readonly IMainViewModel _mainViewModel;
        private readonly Lazy<INewSubjectViewModel> _newSubjectViewModel;

        private int? _subjectId;
        private int? _number;
        private string? _task;
        private byte[]? _file;
        private ICollection<LightSubject>? _subjects;

        public ICommand SubjectCommand { get; set; }
        public ICommand NumberCommand { get; set; }
        public ICommand TaskCommand { get; set; }
        public ICommand FileCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand NewSubjectViewCommand { get; set; }


        public NewExerciseViewModel(ApiExerciseService exerciseiService, ApiSubjectService subjectService, Lazy<INewSubjectViewModel> newSubjectViewModel, IMainViewModel mainViewModel)
        {
            _exerciseService = exerciseiService;
            _subjectService = subjectService;
            _mainViewModel = mainViewModel;
            _newSubjectViewModel = newSubjectViewModel;

            _subjectId = 1;
            _number = 1;
            _task = string.Empty;
            _file = null;

            SubjectCommand = new RelayCommand(x => SubjectId = (int?)x);
            NumberCommand = new RelayCommand(x => Number = (int?)x);
            TaskCommand = new RelayCommand(x => Task = (string)x);
            FileCommand = new RelayCommand(_ => File = ZipFileStreamer.SetFile());
            NewSubjectViewCommand = new RelayCommand(_ => OpenNewSubject());
            SaveCommand = new AsyncRelayCommand(async _ => await CreateNewExerciseAsync());

            LoadSubjects();
        }

        public ICollection<LightSubject>? Subjects
        {
            get => _subjects;
            set
            {
                _subjects = value;
                OnPropertyChanged();
            }
        }

        public int? SubjectId
        {
            get => _subjectId;
            set
            {
                _subjectId = value;
                OnPropertyChanged();
            }
        }

        public int? Number
        {
            get => _number;
            set {
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

        public byte[]? File
        {
            get => _file;
            set
            {
                _file = value;
                OnPropertyChanged();
            }
        }

        public async void LoadSubjects()
        {
            Subjects = await _subjectService.GetAllAsync();
        }

        public async Task CreateNewExerciseAsync()
        {
            if (File != null && SubjectId != null && Task != string.Empty && Number != null)
            {
                var exercise = new NewExercise
                {
                    SubjectId = 1,
                    Number = Number,
                    Task = Task,
                    Files = new ExercisesTasksFile()
                    {
                        TasksFile = File
                    }
                };

                var error = await _exerciseService.AddExerciseAsync(exercise);

                SubjectId = 1;
                Number = 1;
                Task = string.Empty;
                File = null;
            }
        }

        public void OpenNewSubject()
        {
            _mainViewModel.ChangeCurrentView(_newSubjectViewModel.Value);
        }
    }
}
