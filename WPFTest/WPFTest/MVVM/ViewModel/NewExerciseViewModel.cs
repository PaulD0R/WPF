using System.Windows.Input;
using WPFTest.ApiServices;
using WPFTest.Core;
using WPFTest.Exeptions;
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

        private int? _subjectId = 1;
        private int? _number = 1;
        private string? _task = string.Empty;
        private byte[]? _file = [];
        private ICollection<LightSubject>? _subjects = [];
        private bool? _isError = false;
        private string? _errorText = string.Empty;

        public ICommand FileCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand NewSubjectViewCommand { get; set; }


        public NewExerciseViewModel(ApiExerciseService exerciseiService, ApiSubjectService subjectService, IMainViewModel mainViewModel)
        {
            _exerciseService = exerciseiService;
            _subjectService = subjectService;

            _mainViewModel = mainViewModel;

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

        public bool? IsError 
        {
            get => _isError;
            set
            {
                _isError = value;
                OnPropertyChanged();
            }
        }

        public string? ErrorText
        {
            get => _errorText;
            set
            {
                _errorText = value;
                OnPropertyChanged();
            }
        }

        public async void LoadSubjects()
        {
            try
            {
                Subjects = await _subjectService.GetAllAsync();
            }
            catch (ApiExeption ex)
            {
                _mainViewModel.OpenErrorView(ex.Message);
            }
        }

        public async Task CreateNewExerciseAsync()
        {
            try
            {
                if (File != null && SubjectId != null && Task != string.Empty && Number != null)
                {
                    var exercise = new NewExercise
                    {
                        SubjectId = SubjectId,
                        Number = Number,
                        Task = Task,
                        Files = new ExercisesTasksFile()
                        {
                            TasksFile = File
                        }
                    };

                    if (!await _exerciseService.AddExerciseAsync(exercise))
                    {
                        ErrorText = "Неудалось создать упражнение";
                        IsError = true;
                        return;
                    }

                    SubjectId = 1;
                    Number = 1;
                    Task = string.Empty;
                    File = null;
                    IsError = false;
                }
                else
                {
                    ErrorText = "Заполните все поля";
                    IsError = true;
                }
            }
            catch (ApiExeption ex)
            {
                _mainViewModel.OpenErrorView(ex.Message);
            }
        }

        public void OpenNewSubject()
        {
            try
            {
                _mainViewModel.OpenNewSubjectView();
            }
            catch (ApiExeption ex)
            {
                _mainViewModel.OpenErrorView(ex.Message);
            }
        }
    }
}
