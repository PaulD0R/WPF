using System.Windows.Input;
using WPFTest.ApiServices.Interfaces;
using WPFTest.Core;
using WPFTest.Exeptions;
using WPFTest.FileStreamers;
using WPFTest.MVVM.Model.Exercise;
using WPFTest.MVVM.Model.Files;
using WPFTest.MVVM.Model.Subject;
using WPFTest.MVVM.ViewModel.Interfaces;
using WPFTest.Services.Interfaces;

namespace WPFTest.MVVM.ViewModel
{
    public class NewExerciseViewModel : ObserverItem, INewExerciseViewModel
    {
        private readonly IApiExerciseService _exerciseService;
        private readonly IApiSubjectService _subjectService;
        private readonly INavigationService _navigationService;

        private int? _subjectId = 1;
        private int? _number = 1;
        private string? _task = string.Empty;
        private byte[]? _file = [];
        private ICollection<LiteSubject>? _subjects = [];
        private bool? _isError = false;
        private string? _errorText = string.Empty;

        public ICommand FileCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand NewSubjectViewCommand { get; }


        public NewExerciseViewModel(IApiExerciseService exerciseiService, IApiSubjectService subjectService, 
            INavigationService navigationService)
        {
            _exerciseService = exerciseiService;
            _subjectService = subjectService;
            _navigationService = navigationService;

            FileCommand = new RelayCommand(_ => File = ZipFileStreamer.SetFile());
            NewSubjectViewCommand = new RelayCommand(_ => OpenNewSubject());
            SaveCommand = new AsyncRelayCommand(async _ => await CreateNewExerciseAsync());
        }

        public ICollection<LiteSubject>? Subjects
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

        public async Task LoadSubjects()
        {
            Subjects = await _subjectService.GetAllAsync();
        }

        private async Task CreateNewExerciseAsync()
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
            catch (ApiException ex)
            {
                _navigationService.NavigateTo<IErrorViewModel>(x => x.LoadError(ex.Message));
            }
        }

        private void OpenNewSubject()
        {
            try
            {
                _navigationService.NavigateTo<INewSubjectViewModel>();
            }
            catch (ApiException ex)
            {
                _navigationService.NavigateTo<IErrorViewModel>(x => x.LoadError(ex.Message));
            }
        }
    }
}
